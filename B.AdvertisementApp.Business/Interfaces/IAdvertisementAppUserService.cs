using B.AdvertisementApp.Common;
using B.AdvertisementApp.Common.Enums;
using B.AdvertisementApp.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.AdvertisementApp.Business.Interfaces
{
    public interface IAdvertisementAppUserService
    {
        Task<IResponse<AdvertisementAppUserCreateDto>> CreateAsync(AdvertisementAppUserCreateDto advertisementAppUserCreateDto);
        Task<List<AdvertisementAppUserListDto>> GetList(AdvertisementAppUserStatusType statusType);
        Task SetStatusAsync(int advertisementAppUserId, AdvertisementAppUserStatusType type);
    }
}
