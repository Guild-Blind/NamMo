using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaZeroScene2 : BaseScene
{
    [SerializeField] private Transform _spawnPos1;
    [SerializeField] private Transform _spawnPos2;
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.KatanaZeroScene2;

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
            if (Managers.Scene.LastLocatedScene == Define.Scene.KatanaZeroScene1)
            {
                player.transform.position = _spawnPos1.position;
            }
            else if (Managers.Scene.LastLocatedScene == Define.Scene.KatanaZeroScene3)
            {
                player.transform.position = _spawnPos2.position;
            }
            else if (Managers.Scene.LastLocatedScene == Define.Scene.MainScene)
            {
                // ���ξ����� �̾��ϱ⸦ ���� ������ ��� ����Ǿ��ִ� �÷��̾��� ��ǥ��
                player.transform.position = playerData.Position;
            }
        }

        SpawnEnemies();
    }
    public override void Clear()
    {
    }
}
