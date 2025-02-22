﻿using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Events;
using Sanaap.App.Services.Contracts;
using Sanaap.App.Views.Comment;
using Sanaap.Constants;
using Sanaap.Dto;
using Sanaap.Enums;
using Sanaap.Service.Contracts;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.Comment
{
    public class CommentListViewModel : BitViewModelBase
    {
        private readonly IODataClient _oDataClient;
        private readonly IUserDialogs _userDialogs;
        private readonly ICommentValidator _commentValidator;
        private ICommentService _commentService;
        public CommentListViewModel(IODataClient oDataClient,
            ICommentValidator commentValidator,
            ISanaapAppTranslateService translateService,
            IUserDialogs userDialogs,
            IEventAggregator eventAggregator,
            ICommentService commentService,
            IPageDialogService pageDialogService)
        {
            _oDataClient = oDataClient;
            _userDialogs = userDialogs;
            _commentService = commentService;

            CommentTypes = EnumHelper<CommentType>.GetDisplayValues(CommentType.Complaint);

            SelectedCommentType = CommentTypes[1];

            CreateComment = new BitDelegateCommand(async () =>
            {
                submitCancellationTokenSource?.Cancel();
                submitCancellationTokenSource = new CancellationTokenSource();

                using (userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: submitCancellationTokenSource.Cancel))
                {
                    Comment.CommentType = (CommentType)CommentTypes.IndexOf(SelectedCommentType);

                    if (!commentValidator.IsValid(Comment, out string errorMessage))
                    {
                        await pageDialogService.DisplayAlertAsync(string.Empty, translateService.Translate(errorMessage), ConstantStrings.Ok);
                        return;
                    }

                    Comment = await commentService.AddAsync(Comment);

                    await pageDialogService.DisplayAlertAsync(string.Empty, ConstantStrings.SuccessfulProcess, ConstantStrings.Ok);

                    Comment = new CommentDto();

                    eventAggregator.GetEvent<OpenCreateCommentPopupEvent>().Publish(new OpenCreateCommentPopupEvent());

                    await loadComments();
                }
            });

            ShowComment = new BitDelegateCommand<CommentItemSource>(async (comment) =>
              {
                  if (string.IsNullOrEmpty(comment.Answer))
                  {
                      comment.Answer = ConstantStrings.ResponseNotFoundFromSupport;
                  }

                  await NavigationService.NavigateAsync(nameof(CommentAnswerPopupView), new NavigationParameters
                  {
                      {nameof(Comment),comment }
                  });
              });

            OpenCreatePopup = new BitDelegateCommand(async () =>
              {
                  eventAggregator.GetEvent<OpenCreateCommentPopupEvent>().Publish(new OpenCreateCommentPopupEvent());
              });
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            submitCancellationTokenSource?.Cancel();
            submitCancellationTokenSource = new CancellationTokenSource();

            using (_userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: submitCancellationTokenSource.Cancel))
            {
                await loadComments();
            }
        }
        public ObservableCollection<CommentItemSource> Comments { get; set; }

        public BitDelegateCommand CreateComment { get; set; }

        public BitDelegateCommand<CommentItemSource> ShowComment { get; set; }

        public List<string> CommentTypes { get; set; }

        public CommentDto Comment { get; set; } = new CommentDto();

        public BitDelegateCommand OpenCreatePopup { get; set; }


        private CancellationTokenSource submitCancellationTokenSource;

        public string SelectedCommentType { get; set; }

        public async Task loadComments()
        {
            IEnumerable<CommentItemSource> comments = await _commentService.LoadComments();

            if (comments != null)
            {
                Comments = new ObservableCollection<CommentItemSource>(comments);
            }
        }
    }

    public class CommentItemSource : CommentDto
    {
        public string StatusTypeName => EnumHelper<StatusType>.GetDisplayValue(StatusType);
    }
}
