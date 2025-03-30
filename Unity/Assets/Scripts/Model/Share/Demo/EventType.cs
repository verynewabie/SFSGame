using System.Collections.Generic;
using Box2DSharp.Dynamics;
using Unity.Mathematics;

namespace ET.Client
{
    public struct SceneChangeStart
    {
    }
    
    public struct SceneChangeFinish
    {
    }
    
    public struct AfterCreateClientScene
    {
    }
    
    public struct AfterCreateCurrentScene
    {
    }

    public struct AppStartInitFinish
    {
    }

    public struct LoginFinish
    {
    }
    
    public struct EnterMapFinish
    {
    }

    public struct AfterUnitCreate
    {
        public Unit Unit;
    }

    public struct ShowUIHint
    {
        public string hint;
        public bool showCloseBtn;
    }

    public struct ReplayGame
    {
        public List<SFSUnitInfo> units;
        public long battleId;
    }

    public struct HideUIHint
    {
        
    }

    public struct PlayerEnterRoom
    {
        public string name;
        public long playerId;
    }

    public struct SFSLoadScene
    {
        public string sceneName;
    }

    public struct Reconnect
    {
        
    }

    public struct CreateSFSUnit
    {
        public bool IsLocalPlayer;
        public SFSUnit unit;
        // public bool HeroConfigId;
    }

    public struct CreateSFSProjectile
    {
        public SFSUnit unit;
    }

    public struct SFSLoadSceneDone
    {
        
    }

    public struct RemoveReplayUI
    {
        
    }

    public struct GameEnd
    {
        
    }

    public struct InitBattleView
    {
        public PlayerInputComponent PlayerInputComponent;
    }

    public struct InitReplayBattleView
    {
        
    }

    public struct RemoveUnitView
    {
        public List<long> UnitToDelete;
    }

    public struct ShowDebugInfo
    {
        public List<float3> Pos;
        public List<float> Radius;
    }
}

namespace ET
{
    public struct AddCmdToSendQueue
    {
        public IRoomCmd Cmd;
    }

    public struct AddUnitToRemove
    {
        public long UnitId;
    }

    public struct AddBodyToRemove
    {
        public Body Body;
    }
}