using ColorSorting3D.Gameplay;

namespace ColorSorting3D.Data
{
    public class ProgressModel : DataModel<ProgressDTO>
    {
        protected override void SetKey()
            => dataKey = DATA_KEY.PROGRESS;

        protected override ProgressDTO CreateDefaultDTO()
            => new() { CurrentLevel = 1, };
    }
}