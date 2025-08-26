using UnityEngine;

public class Bootstrap : MonoBehaviour
{
private void Awake()
{
Application.targetFrameRate = 120;
var gm = FindObjectOfType<GameManager>();
if (gm != null) ServiceLocator.Register(gm);


var spawner = FindObjectOfType<EnemySpawner>();
if (spawner != null) ServiceLocator.Register(spawner);


var ui = FindObjectOfType<UIManager>();
if (ui != null) ServiceLocator.Register(ui);
}
}
