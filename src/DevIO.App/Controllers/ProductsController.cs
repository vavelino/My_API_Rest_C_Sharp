using AutoMapper;
using DevIO.App.ViewModels;
using DevIO.Business.Interface;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DevIO.App.Controllers
{
    //[Authorize]
    [Route("produtos")]
    public class ProductsController : MainController
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;      
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository,                              
                                  IProductService productService,
                                  IMapper mapper,
                                  INotifier notifier) : base(notifier)
        {
            _productService = productService;
            _productRepository = productRepository;   
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetProductsSuppliers());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductViewModel>> GetById(Guid id)
        {
            var productViewModel = await GetProductById(id);

            if (productViewModel == null) return NotFound();

            return productViewModel;
        }
        [HttpPost]
        public async Task<ActionResult<ProductViewModel>> Create(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var imageName = Guid.NewGuid() + "_" + productViewModel.Image;
            if (!UploadFile(productViewModel.ImageUpload, imageName))
            {
                return CustomResponse(productViewModel);
            }
            productViewModel.Image = imageName;
            await _productService.Add(_mapper.Map<Product>(productViewModel));

            return CustomResponse(productViewModel);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id)
            {
                NotifyError("Os ids informados não são iguais!");
                return CustomResponse();
            }

            var productUpdate = await GetProductById(id);

            if (string.IsNullOrEmpty(productViewModel.Image))
                productViewModel.Image = productUpdate.Image;

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (productViewModel.ImageUpload != null)
            {
                var imagemNome = Guid.NewGuid() + "_" + productViewModel.Image;
                if (!UploadFile(productViewModel.ImageUpload, imagemNome))
                {
                    return CustomResponse(ModelState);
                }

                productUpdate.Image = imagemNome;
            }

            productUpdate.SupplierId = productViewModel.SupplierId;
            productUpdate.Name = productViewModel.Name;
            productUpdate.Description = productViewModel.Description;
            productUpdate.Value = productViewModel.Value;
            productUpdate.Active = productViewModel.Active;

            await _productService.Update(_mapper.Map<Product>(productUpdate));

            return CustomResponse(productViewModel);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProductViewModel>> Delete(Guid id)
        {
            var produto = await GetProductById(id);

            if (produto == null) return NotFound();

            await _productService.Remove(id);

            return CustomResponse(produto);
        }
        private async Task<ProductViewModel> GetProductById(Guid id)
        {
            return _mapper.Map<ProductViewModel>(await _productRepository.GetProductSupplier(id));
        }

        private bool UploadFile(string file, string imgName)
        {
            if (string.IsNullOrEmpty(file))
            {
                NotifyError("Forneça uma imagem para este produto!");
                return false;
            }

            var imageDataByteArray = Convert.FromBase64String(file);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imgName);

            if (System.IO.File.Exists(filePath))
            {
                NotifyError("Já existe um arquivo com este nome!");
                return false;
            }

            System.IO.File.WriteAllBytes(filePath, imageDataByteArray);

            return true;
        }
    }
}
