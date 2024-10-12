using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaZeroScene1 : BaseScene
{
    [SerializeField] private Transform _spawnPos1;
    [SerializeField] private Transform _spawnPos2;
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.KatanaZeroScene1;

        GameObject player = SpawnPlayer();

        PlayerData playerData = Managers.Data.PlayerData;
        if (PlayerData.Respawn)
        {
            PlayerData.Respawn = false;
            player.transform.position = playerData.Position;
        }
        else
        {
            // ���� �÷ο��� ���� ������ ������ ��� ������ ��ǥ��
            if (Managers.Scene.LastLocatedScene == Define.Scene.TestStartScene)
            {
                player.transform.position = _spawnPos1.position;
            }
            else if(Managers.Scene.LastLocatedScene == Define.Scene.KatanaZeroScene2)
            {
                player.transform.position = _spawnPos2.position;
            }
            else if (Managers.Scene.LastLocatedScene == Define.Scene.MainScene)
            {
                // ���ξ����� �̾��ϱ⸦ ���� ������ ��� ����Ǿ��ִ� �÷��̾��� ��ǥ��
                player.transform.position = playerData.Position;
            }
        }

        Dictionary<int, Data.Enemy> enemyDict = Managers.Data.EnemyDict;
        Data.StageEnemy stageEnemy = Managers.Data.EnemyData.stageEnemies[(int)SceneType];
        foreach(var enemy in stageEnemy.enemies)
        {
            if (enemy.alive == false) continue;
            string prefabPath = enemyDict[enemy.enemyId].prefabPath;
            GameObject go = Managers.Resource.Instantiate(prefabPath);
            go.transform.position = new Vector2(enemy.posX, enemy.posY);
        }
    }
    public override void Clear()
    {
    }
}
