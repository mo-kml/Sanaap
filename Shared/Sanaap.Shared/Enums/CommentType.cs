using System.ComponentModel.DataAnnotations;

namespace Sanaap.Enums
{
    public enum CommentType
    {
        [Display(Name = "انتقاد")]
        Criticism,

        [Display(Name = "پیشنهاد")]
        Suggestion,

        [Display(Name = "شکایت")]
        Complaint
    }
}
