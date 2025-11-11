

public class State
{

    public enum XiaState
    {
        Follow,
        Sequencer,        // 正在移动到屏幕中心（协程期间）
        SequencerLocked,  // 已在屏幕中心 —— x,y 每帧绑定 camera 中心
        Fixed
    }

    // 宠物状态枚举
    public enum PetState
    {
        Chasing,    // 追击状态（安全范围外）
        Edging,     // 边缘状态
        SafeZone    // 安全范围内
    }
}
