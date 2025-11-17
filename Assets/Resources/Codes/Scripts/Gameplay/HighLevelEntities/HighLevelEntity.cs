using System.Threading.Tasks;
using UnityEngine;

namespace ColorSorting3D.Gameplay
{
    public abstract class HighLevelEntity : MonoBehaviour
    {
        public abstract Task Init();
    }
}