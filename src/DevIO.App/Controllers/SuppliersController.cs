using AutoMapper;
using DevIO.App.Extensions;
using DevIO.App.ViewModels;
using DevIO.Business.Models;
using DevIO.Business.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevIO.App.Controllers
{
    [Route("fornecedores")]
    public class SuppliersController : MainController
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;

        public SuppliersController(ISupplierRepository supplierRepository,
                                   ISupplierService supplierService,
                                   IAddressRepository addressRepository,
                                   IMapper mapper,
                                   INotifier notifier) : base(notifier)
        {
            _supplierRepository = supplierRepository;
            _supplierService = supplierService;
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<SupplierViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierRepository.GetAll());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> GetById(Guid id)
        {
            var supplier = await GetSupplierProductAddress(id);

            if (supplier == null) return NotFound();

            return supplier;
        }

        [HttpPost]
        public async Task<ActionResult<SupplierViewModel>> Create(SupplierViewModel supplierViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _supplierService.Add(_mapper.Map<Supplier>(supplierViewModel));

            return CustomResponse(supplierViewModel);
        }
       
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> Update(Guid id, SupplierViewModel supplierViewModel)
        {
            // if (id != supplierViewModel.Id)   return BadRequest();
            if (id != supplierViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(supplierViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _supplierService.Update(_mapper.Map<Supplier>(supplierViewModel));

            return CustomResponse(supplierViewModel);
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> Delete(Guid id)
        {
            var supplierViewModel = await GetSupplierAddress(id);

            if (supplierViewModel == null) return NotFound();

            await _supplierService.Remove(id);
            //return CustomResponse(supplierViewModel); Se não quiser passar o objeto que excluiu

            return CustomResponse(supplierViewModel);
        }

        [HttpGet("endereco/{id:guid}")]
        public async Task<SupplierViewModel> GetAddressById(Guid id)
        {
            return _mapper.Map<SupplierViewModel>(await _addressRepository.GetByID(id));
        }
        
        [HttpPut("endereco/{id:guid}")]
        public async Task<IActionResult> UpdateAddressById(Guid id, AddressViewModel addressViewModel)
        {
            if (id != addressViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(addressViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _supplierService.UpdateAdress(_mapper.Map<Address>(addressViewModel));

            return CustomResponse(addressViewModel);
        }
        private async Task<SupplierViewModel> GetSupplierAddress(Guid id)
        {
            return _mapper
                .Map<SupplierViewModel>
                (await _supplierRepository.GetSupplierAddressByID(id));
        }
        private async Task<SupplierViewModel> GetSupplierProductAddress(Guid id)
        {
            var SupplierProductAddress = _mapper
                .Map<SupplierViewModel>
                (await _supplierRepository.GetSupplierProductAddressByID(id));
            return SupplierProductAddress;
        }
    }
}