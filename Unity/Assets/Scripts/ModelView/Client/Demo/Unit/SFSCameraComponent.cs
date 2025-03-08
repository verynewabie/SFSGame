namespace ET.Client
{
    
    [ComponentOf(typeof(BattleRoom))]
    public class SFSCameraComponent:Entity,IAwake<SFSUnitView>
    {
        private EntityRef<SFSUnitView> unitView;

        public SFSUnitView UnitView
        {
            get{ return unitView; }
            set{ unitView = value; }
        }
        
    }
}
