using Bit.Model;
using Bit.Model.Contracts;
using System;
using System.Collections.Generic;

namespace Sanaap.Dto
{
    public class NewsList
    {
        public List<ContentDto> Items { get; set; }
    }
    public class ContentDto : Bindable, IDto
    {
        public int NewsID { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public string Photo { get; set; }

        public int Visits { get; set; }

        public int Likes { get; set; }

        public bool YourLike { get; set; }

        public virtual Guid Id { get; set; }

        public virtual DateTimeOffset CreatedOn { set; get; }

        public virtual DateTimeOffset ModifiedOn { set; get; }
    }
}
