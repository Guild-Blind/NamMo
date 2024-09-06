using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaZeroScene4 : BaseScene
{
    [SerializeField] private Transform _spawnPos1;
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.KatanaZeroScene4;

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
            if (Managers.Scene.LastLocatedScene == Define.Scene.KatanaZeroScene3)
            {
                player.transform.position = _spawnPos1.position;
            }
            else if (Managers.Scene.LastLocatedScene == Define.Scene.MainScene)
            {
                // ���ξ����� �̾��ϱ⸦ ���� ������ ��� ����Ǿ��ִ� �÷��̾��� ��ǥ��
                player.transform.position = playerData.Position;
            }
        }
    }
    public override void Clear()
    {
    }
}
