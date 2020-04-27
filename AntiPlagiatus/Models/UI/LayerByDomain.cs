using System.Collections.Generic;

namespace AntiPlagiatus.Models.UI
{
    public class LayerByDomain : Base
    {
        private bool isSelected;

        public int Rewrite { get; set; }
        public List<int> Words { get; set; }
        public string Uri { get; set; }
        public int Equality { get; set; }
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (this.isSelected != value)
                {
                    this.isSelected = value;
                    this.RaisePropertyChanged();
                }
            }
        }
    }
}
