namespace Sanaap.Dto
{
    //تمام موجودیت هایی که از سایت بیمه خوانده میشوند از طریق این کلاس تبدیل خواهند شد مثال : رنگ ها - اسم ماشین ها و ...
    public class ExternalEntityDto
    {
        public int PrmID { get; set; }

        public string Name { get; set; }

        public int? Type { get; set; }

        public int? ChildOf { get; set; }
    }
}
