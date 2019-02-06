﻿using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Events;
using Prism.Navigation;
using Sanaap.App.Events;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.App.Views.Content;
using Sanaap.Constants;
using Sanaap.Dto;
using Simple.OData.Client;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.Content
{
    public class ContentListViewModel : BitViewModelBase
    {
        private readonly IODataClient _oDataClient;
        private readonly IUserDialogs _userDialogs;
        private readonly INewsService _newsService;
        private readonly IInitialDataService _initialDataService;
        public ContentListViewModel(IODataClient oDataClient, IUserDialogs userDialogs, INewsService newsService, IInitialDataService initialDataService, IEventAggregator eventAggregator)
        {
            _oDataClient = oDataClient;
            _userDialogs = userDialogs;
            _newsService = newsService;
            _initialDataService = initialDataService;

            ShowContent = new BitDelegateCommand<NewsItemSource>(async (content) =>
              {
                  INavigationParameters parameters = new NavigationParameters();
                  parameters.Add("NewsId", content.NewsID);

                  await NavigationService.NavigateAsync(nameof(ShowContentView), parameters);
              });

            FilterContent = new BitDelegateCommand(async () =>
              {
                  using (_userDialogs.Loading(ConstantStrings.Loading))
                  {
                      await loadContents(FilterDto);
                  }

                  eventAggregator.GetEvent<OpenNewsFilterPopupEvent>().Publish(new OpenNewsFilterPopupEvent());

                  FilterDto = new FilterNewsDto();
              });

            OpenFilterPopup = new BitDelegateCommand(async () =>
              {
                  eventAggregator.GetEvent<OpenNewsFilterPopupEvent>().Publish(new OpenNewsFilterPopupEvent());
              });
        }
        public ObservableCollection<NewsItemSource> Contents { get; set; }

        public BitDelegateCommand<NewsItemSource> ShowContent { get; set; }

        public MonthItemSource[] Months { get; set; }
        public MonthItemSource SelectedMonth { get; set; }

        public YearItemSource[] Years { get; set; }
        public YearItemSource SelectedYear { get; set; }

        public FilterNewsDto FilterDto { get; set; } = new FilterNewsDto();

        public BitDelegateCommand FilterContent { get; set; }

        public BitDelegateCommand OpenFilterPopup { get; set; }
        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            using (_userDialogs.Loading(ConstantStrings.Loading))
            {
                await loadContents(FilterDto);

                Months = await _initialDataService.GetMonths();

                Years = await _initialDataService.GetYears();
            }
        }

        public async Task loadContents(FilterNewsDto filterNewsDto)
        {
            if (SelectedMonth != null)
            {
                filterNewsDto.Month = SelectedMonth.Number;
            }

            if (SelectedYear != null)
            {
                filterNewsDto.Year = SelectedYear.Number;
            }

            List<NewsItemSource> contents = await _newsService.GetNews(filterNewsDto);

            if (contents != null)
            {
                Contents = new ObservableCollection<NewsItemSource>(contents);
            }
        }
    }
}
