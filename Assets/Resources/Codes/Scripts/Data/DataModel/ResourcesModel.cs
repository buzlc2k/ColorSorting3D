using ColorSorting3D.Gameplay;

namespace ColorSorting3D.Data
{
    public class ResourcesModel : DataModel<UserResourcesDTO>
    {
        protected override void SetKey()
            => dataKey = DATA_KEY.RESOURCES;

        protected override UserResourcesDTO CreateDefaultDTO()
            => new() { CurrentCoins = 0, CurrentUndoItems = 0 };
    }
}