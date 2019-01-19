using Acr.UserDialogs;
using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Services;
using Sanaap.Constants;
using Sanaap.Dto;
using Sanaap.Enums;
using Sanaap.Service.Contracts;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Sanaap.App.ViewModels.Comment
{
    public class CreateCommentViewModel : BitViewModelBase
    {
        public CreateCommentViewModel(
            ICommentValidator commentValidator,
            IUserDialogs userDialogs,
            IPageDialogService pageDialogService,
            ISanaapAppTranslateService translateService,
            IODataClient oDataClient,
            INavService navigationService)
        {
            CommentTypes = EnumHelper<CommentType>.GetDisplayValues(CommentType.Complaint);

            SelectedCommentType = CommentTypes[1];

            Submit = new BitDelegateCommand(async () =>
            {
                submitCancellationTokenSource?.Cancel();
                submitCancellationTokenSource = new CancellationTokenSource();

                using (userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: submitCancellationTokenSource.Cancel))
                {
                    Comment.CommentType = (CommentType)CommentTypes.IndexOf(SelectedCommentType);

                    if (!commentValidator.IsValid(Comment, out string errorMessage))
                    {
                        await pageDialogService.DisplayAlertAsync("", translateService.Translate(errorMessage), "باشه");
                        return;
                    }

                    Comment = await oDataClient.For<CommentDto>("Comments")
                                .Set(Comment)
                                .InsertEntryAsync();



                    await pageDialogService.DisplayAlertAsync("", "پیام شما با موفقیت ثبت شد.", "باشه");

                    await navigationService.GoBackAsync();
                }
            });
        }
        public List<string> CommentTypes { get; set; }

        public CommentDto Comment { get; set; } = new CommentDto();

        public BitDelegateCommand Submit { get; set; }

        private CancellationTokenSource submitCancellationTokenSource;

        public string SelectedCommentType { get; set; }
    }
}
