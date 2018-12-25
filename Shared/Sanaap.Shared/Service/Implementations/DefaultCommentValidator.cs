using Sanaap.Dto;
using Sanaap.Service.Contracts;
using System;
using System.Text.RegularExpressions;

namespace Sanaap.Service.Implementations
{
    public class DefaultCommentValidator : ICommentValidator
    {
        public bool IsValid(CommentDto comment, out string message)
        {
            if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment));
            }

            if (string.IsNullOrEmpty(comment.FirstName))
            {
                message = $"{nameof(CommentDto.FirstName)}IsEmpty";
                return false;
            }

            if (string.IsNullOrEmpty(comment.LastName))
            {
                message = $"{nameof(CommentDto.LastName)}IsEmpty";
                return false;
            }

            if (string.IsNullOrEmpty(comment.Mobile))
            {
                message = $"{nameof(CommentDto.Mobile)}IsEmpty";
                return false;
            }

            comment.Mobile = comment.Mobile.Trim();

            if (!IsValidMobileNumber(comment.Mobile))
            {
                message = $"{nameof(CommentDto.Mobile)}IsInvalid";
                return false;
            }

            message = null;

            return true;
        }

        public virtual bool IsValidMobileNumber(string input)
        {
            if (!Regex.IsMatch(input, @"^[0][9][0-9][0-9]{8,8}$"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
