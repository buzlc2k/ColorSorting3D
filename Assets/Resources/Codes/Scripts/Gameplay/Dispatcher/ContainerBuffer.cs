namespace ColorSorting3D.Gameplay
{
    public class ContainerBuffer
    {
        public RingsContainer Source { get; set; }
        public RingsContainer Dest { get; set; }

        public void Clear()
        {
            Source = null;
            Dest = null;
        }
    }
}