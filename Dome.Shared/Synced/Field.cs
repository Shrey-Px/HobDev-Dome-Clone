namespace Dome.Shared.Synced
{
    /// <summary>
    /// field of a sport.e.g a court for a badminton. the booking is done on a field.
    /// Embedded inside AvailableGame.
    /// </summary>
    public partial class Field : IEmbeddedObject
    {
        private Field() { }

        public Field(int fieldNumber, string fieldType)
        {
            FieldNumber = fieldNumber;
            FieldType = fieldType;
        }

        [MapTo("fieldNumber")]
        public int FieldNumber { get; set; }

        [MapTo("fieldType")]
        public string FieldType { get; set; }

        //Local
        [Ignored]
        public string FieldName => $"{FieldType} {FieldNumber.ToString()}";

        private bool isSelected;

        // used to highlight the selected field while booking in the vendor app
        [Ignored]
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
