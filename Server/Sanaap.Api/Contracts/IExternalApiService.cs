﻿using Sanaap.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Sanaap.Api.Controllers.EvlRequestExpertsController;

namespace Sanaap.Api.Contracts
{
    public interface IExternalApiService
    {
        Task<IEnumerable<ExternalEntityDto>> GetColors();

        Task<IEnumerable<ExternalEntityDto>> GetCars();

        Task<IEnumerable<InsurerDto>> GetInsurers();

        Task<IEnumerable<ContentDto>> GetNews(FilterNewsDto filterNewsDto);

        Task<ContentDto> GetNewsById(int id, Guid userId);

        Task<bool> LikeNews(int id, Guid userId);

        Task<IEnumerable<ExternalEntityDto>> GetNumberplateAlphabets();

        Task<IEnumerable<PhotoTypeDto>> GetSalesPhotos();

        Task<IEnumerable<PhotoTypeDto>> GetBadanePhotos();

        Task<ExpertPositionDto> GetExpertPosition(GetPositionArgs positionArgs);

        Task<IEnumerable<ExternalEntityDto>> GetAccidentReasons();

        Task<IEnumerable<EvlRequestProgressDto>> GetFileProgress(int fileId);
    }
}
