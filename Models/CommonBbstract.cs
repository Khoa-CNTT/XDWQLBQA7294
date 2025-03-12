namespace EShopp.Models
{
    public abstract class CommonAbstract
    {
        public string? CreatedBy { get; set; }  // Cho phép NULL
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? Modifiedby { get; set; }  // Cho phép NULL
    }

}
