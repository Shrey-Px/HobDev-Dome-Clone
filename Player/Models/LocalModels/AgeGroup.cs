namespace Player.Models.LocalModels
{
    /// <summary>
    /// the age group of the user. All the age groups are created in memory from the static data from AppStaticData class and user selects one of them as his age group
    /// </summary>
    public partial class AgeGroup : ObservableObject
    {
        public AgeGroup(string ageGroupName)
        {
            AgeGroupName = ageGroupName;
        }

        public string AgeGroupName { get; set; }

        //local
        [ObservableProperty]
        public bool isSelected;
    }
}
