using System;
[Serializable]
public class UserData
{
    // 유저
    public string m_UserName;//유저 이름
    //public string m_UserSex;//유저 성별. Male. Female로 저장
    public string m_UserAvatarSprite;//현재 적용중인 유저의 아바타 이미지

    public string[] m_HaveUserAvatarList; // 유저가 가지고 있는 아바타 이미지 배열
    //public string[] m_HaveUserMaleAvatarList;//유저가 가지고 있는 남성 아바타 이미지 배열
    //public string[] m_HaveUserFemaleAvatarList;//유저가 가지고 있는 여성 아바타 이미지 배열

    public string[] m_HaveUserCharacter; // 유저가 찍은 특성 배열. 배열 크기는 스테이지. 한 스테이지 안에 특성 정보는 +로 해서 묶는다.
    //public string[] m_HaveUserCharacterAuto; // m_UserSex, m_Fame, m_EvilFame에 따라 조건에 따라 자동 적용. 저장하지는 않음.

    // 화폐
    public int m_Gold; // 골드
    public int[] m_GetFame; // 스테이지마다 얻은 명성. 배열 크기는 메인 스테이지 수.
    public int[] m_RemainFame; // 특성을 찍고 남음 명성. 배열 크기는 메인 스테이지 수.
    public int[] m_GetEvilFame; // 스테이지마다 얻은 악명. 배열 크기는 메인 스테이지 수. 
    //public int m_Fame; // 명성
    //public int m_EvilFame; // 악명
}
