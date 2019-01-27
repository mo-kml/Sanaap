using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.Dto;
using Simple.OData.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sanaap.App.Services.Implementations
{
    public class NewsService : INewsService
    {
        private IODataClient _oDataClient;
        public NewsService(IODataClient oDataClient)
        {
            _oDataClient = oDataClient;
        }

        public async Task<List<NewsItemSource>> GetNews()
        {
            List<ContentDto> news = (await _oDataClient.For<ContentDto>("Contents")
                .Function("GetNews")
                .FindEntriesAsync()).ToList();

            List<NewsItemSource> newsItemSources = new List<NewsItemSource>();

            foreach (ContentDto item in news)
            {
                newsItemSources.Add(new NewsItemSource
                {
                    Date = item.Date,
                    Text = item.Text,
                    Id = item.Id,
                    Image = ImageSource.FromUri(new System.Uri(item.Photo)),
                    NewsID = item.NewsID,
                    Photo = item.Photo,
                    Likes = item.Likes,
                    Visits = item.Visits,
                    Title = item.Title,
                    YourLike = item.YourLike
                });
            }

            return newsItemSources;
        }

        public async Task<NewsItemSource> GetNewsById(int id)
        {


            ContentDto news = await _oDataClient.For<ContentDto>("Contents")
                            .Function("GetNewsById")
                            .Set(new { newsId = id })
                            .FindEntryAsync();

            return new NewsItemSource
            {
                Date = news.Date,
                Text = news.Text,
                Id = news.Id,
                Image = ImageSource.FromUri(new System.Uri(news.Photo)),
                NewsID = news.NewsID,
                Photo = news.Photo,
                Likes = news.Likes,
                Visits = news.Visits,
                Title = news.Title,
                YourLike = news.YourLike
            };
        }
    }
}
