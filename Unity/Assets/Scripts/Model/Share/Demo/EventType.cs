using System.Buffers.Binary;

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

    public struct CreateSFSUnit
    {
        public bool IsLocalPlayer;
        public SFSUnit unit;
        // public bool HeroConfigId;
    }

    public struct SFSLoadSceneDone
    {
        
    }

    public struct InitBattleView
    {
        public PlayerInputComponent PlayerInputComponent;
    }
}

namespace ET
{
    public struct AddCmdToSendQueue
    {
        public IRoomCmd Cmd;
    }
}