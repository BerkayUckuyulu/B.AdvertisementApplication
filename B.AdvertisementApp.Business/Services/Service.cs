using AutoMapper;
using B.AdvertisementApp.Business.Extensions;
using B.AdvertisementApp.Business.Interfaces;
using B.AdvertisementApp.Common;
using B.AdvertisementApp.DataAccess.UnitOfWork;
using B.AdvertisementApp.Dtos;
using B.AdvertisementApp.Entities;

using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.AdvertisementApp.Business.Services
{
    public class Service<CreateDto, UpdateDto, ListDto,T> : IService<CreateDto, UpdateDto, ListDto,T> 
        where CreateDto : class, IDto, new()
        where UpdateDto : class, IUpdateDto, new() 
        where ListDto : class, IDto, new()
        where T:BaseEntity
    {
        private readonly IMapper _mapper;
        private readonly IValidator<CreateDto> _createDtoValidator;
        private readonly IValidator<UpdateDto> _updateDtoValidator;
        private readonly IUow _uow;

        public Service(IMapper mapper, IValidator<CreateDto> createDtoValidator, IUow uow, IValidator<UpdateDto> updateDtoValidator)
        {
            _mapper = mapper;
            _createDtoValidator = createDtoValidator;
            _uow = uow;
            _updateDtoValidator = updateDtoValidator;
        }

        public async Task<IResponse<CreateDto>> CreateAsync(CreateDto createDto)
        {
            var result=_createDtoValidator.Validate(createDto);
            if (result.IsValid)
            {
               await _uow.GetRepository<T>().CreateAsync(_mapper.Map<T>(createDto));
                return new Response<CreateDto>(ResponseType.Success, createDto);
            }
            else
            {
                return  new Response<CreateDto>(createDto,result.ConvertToCustomValidationError());
            }
        }

        public async Task<IResponse<List<ListDto>>> GetAllAsync()
        {
            var data = await _uow.GetRepository<T>().GetAllAsync();
            var dto = _mapper.Map<List<ListDto>>(data);
            return new Response<List<ListDto>>(ResponseType.Success, dto);

        }

        public async Task<IResponse<IDto>> GetByIdAsync<IDto>(int id)
        {
            var data = await _uow.GetRepository<T>().GetByFilterAsync(x=>x.Id==id);
            if (data==null)
            {
                return new Response<IDto>(ResponseType.NotFound, $"{id} ye sahip data bulunamadı");
            }
            else
            {
                var dto = _mapper.Map<IDto>(data);
                return new Response<IDto>(ResponseType.Success, dto);
            }
            
        }

        public async Task<IResponse> RemoveAsync(int id)
        {
            var data = await _uow.GetRepository<T>().FindAsync(id);
            if (data == null)
            {
                return new Response(ResponseType.NotFound, $"{id} ye sahip data bulunamadı");
            }
            else
            {
                _uow.GetRepository<T>().Remove(data);
                return new Response(ResponseType.Success,$"{id}'li data silindi. İşlem başarılı");
            }
        }

        public async Task<IResponse<UpdateDto>> UpdateAsync(UpdateDto updateDto)
        {
            var result = _updateDtoValidator.Validate(updateDto);
            if (result.IsValid)
            {
                var unchangedData = await _uow.GetRepository<T>().FindAsync(updateDto.Id);
                if (unchangedData == null)
                {
                    return new Response<UpdateDto>(ResponseType.NotFound, $"{updateDto.Id} li data bulunamadı");
                }
                else
                {
                    _uow.GetRepository<T>().Update((_mapper.Map<T>(updateDto)), unchangedData);
                    return new Response<UpdateDto>(ResponseType.Success, updateDto);
                }

            }
            return new Response<UpdateDto>(updateDto, result.ConvertToCustomValidationError());
            
        }
    }
}
