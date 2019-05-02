namespace ARWT.ModelInterface.Feet
{
    public interface IFootPlacement : IModelObjectBase
    {
        int centroidX { get; set; }
        int centroidY { get; set; }
        int maxX { get; set; }
        int maxY { get; set; }
        int minX { get; set; }
        int minY { get; set; }
        int width { get; set; }
        int height { get; set; }
    }
}