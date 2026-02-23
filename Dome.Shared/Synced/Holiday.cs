
[MapTo("holiday")]
public partial class Holiday: IEmbeddedObject
{
    private Holiday()
    {
        
    }
    public Holiday(DateTimeOffset date, string description)
    {
        Date=date;
        Description=description;
    }


   [MapTo("date")]
    public DateTimeOffset Date {get;set;}

    [MapTo("description")]
    public string Description {get; set;}
}