using System.Collections.Generic;
using System.Threading.Tasks;
using ColorSorting3D.Config;
using ServiceButcator;
using UnityEngine;

namespace ColorSorting3D.Gameplay
{
    public class GameRoot : MonoBehaviour
    {
        private async void Start()
        {
            await InitGame();

            StartGame();

            void StartGame()
                => ServiceLocators.ThisScene().Get<PresentationTransitionService>().MakeTransition(PresentationTransitionID.LoadingToHome);
        }

        private async Task InitGame()
        {
            var highLevelEntities = FindObjectsByType<HighLevelEntity>(
                                                    FindObjectsInactive.Include,
                                                    FindObjectsSortMode.None);
            var tasks = new List<Task>();
            foreach (var highLevelEntity in highLevelEntities)
                tasks.Add(highLevelEntity.Init());

            await Task.WhenAll(tasks);
        }
    }
}