using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameState {
    
    None = 0,
    Init = 1,

    Logo = 2,

    CopyAssets = 4,

    Launch = 8,
    Login = 10,

    // Lobby  =============
    Main = 100,

    //TrainFieldWindow = Main + 2,
    //BattlePrepareWindow = Main + 4,

    Lobby = 200,

    // Matching=============
    Matching = 300,

    InTeam = Matching + 4,


    MatchConfirming = Matching + 10,

    SelectHero = Matching + 20,

    // GameLoading=============
    GameLoading = 400,

    FirstSetLoading = GameLoading + 10,
    NextSetLoading = GameLoading + 20,
    GveLoading = GameLoading + 30,

    // Gaming=============
    Fighting = 500,
    PVE = Fighting + 2,  //试炼、训练营、引导关卡，离线战斗中
    PVPStage = Fighting + 10, //------ PVP中所有阶段的统称，仅供Gate重连使用

    // result ==========
    InPvpResultWindow = 600,








}
