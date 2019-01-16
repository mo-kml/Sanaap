using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Views.Comment;
using Sanaap.Constants;
using Sanaap.Dto;
using Sanaap.Enums;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.Comment
{
    public class CommentListViewModel : BitViewModelBase
    {
        private readonly IODataClient _oDataClient;
        private readonly IUserDialogs _userDialogs;
        public CommentListViewModel(IODataClient oDataClient,
            INavigationService navigationService,
            IUserDialogs userDialogs,
            IPageDialogService pageDialogService)
        {
            _oDataClient = oDataClient;
            _userDialogs = userDialogs;

            CreateComment = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync(nameof(CreateCommentView));
            });

            ShowComment = new BitDelegateCommand<CommentItemSource>(async (comment) =>
              {
                  await pageDialogService.DisplayAlertAsync("", string.IsNullOrEmpty(comment.Answer) ? "هنوز پاسخی از سمت کارشناسان برای این پیام دریافت نشده است" : comment.Answer, "باشه");
              });
        }

        public override async Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            using (_userDialogs.Loading(ConstantStrings.Loading))
            {
                await loadComments();
            }
        }
        public ObservableCollection<CommentItemSource> Comments { get; set; }

        public BitDelegateCommand CreateComment { get; set; }

        public BitDelegateCommand<CommentItemSource> ShowComment { get; set; }

        public async Task loadComments()
        {
            IEnumerable<CommentDto> comments = (await _oDataClient.For<CommentDto>("Comments")
                .Function("LoadComments")
                .OrderBy(it => it.CreatedOn)
                .FindEntriesAsync());

            if (comments != null)
            {
                Comments = new ObservableCollection<CommentItemSource>(comments.Select(c => new CommentItemSource
                {
                    Answer = c.Answer,
                    AnswerTime = c.AnswerTime,
                    CommentType = c.CommentType,
                    CreatedOn = c.CreatedOn,
                    Description = c.Description,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Id = c.Id,
                    Mobile = c.Mobile,
                    StatusType = c.StatusType,
                    Code = c.Code
                }));
            }
        }
    }

    public class CommentItemSource : CommentDto
    {
        public string StatusTypeName => EnumHelper<StatusType>.GetDisplayValue(StatusType);
    }
}
