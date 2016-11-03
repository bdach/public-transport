namespace PublicTransport.Client.Models
{
    public class MenuItem
    {
        public MenuOption Option { get; set; }
        public string Label { get; set; }

        public MenuItem(string label, MenuOption option)
        {
            Label = label;
            Option = option;
        }

        public override string ToString()
        {
            return Label;
        }
    }

    public enum MenuOption
    {
        City,
        Street
    }
}