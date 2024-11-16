using UnityEngine;
using System.Collections;

public enum NoteMode
{
	None,
	Suggest,
	Warrant,
	SelectCase
}

public enum NoteTap
{
	CASE,
	DIALOG,
	EVIDENCE,
	NEWS
}

public enum CaseMode
{
	Main,
	Side
}

public enum LaboratoryType
{
	/// <summary>
	/// 분석중
	/// </summary>
	Analyzing,
	/// <summary>
	/// 분석 대기중
	/// </summary>
	AnalyzedReverve,
	/// <summary>
	/// 분석 가능
	/// </summary>
	CanAnalyzed,
	/// <summary>
	/// 비교 중
	/// </summary>
	Matching,
	/// <summary>
	/// 비교 가능
	/// </summary>
	CanMatched // 매칭 할 수 있는 아이템 리스트
}

public enum UserActionType
{
	Dialog,
	Search,
	Suggest
}

public enum NewsType
{
	Normal,
	MainQuest,
	SideQuest,
	BreakingNews,
	Special
}
public enum DialogType
{
	Start, // 대화하기 시작 시, 표시되는 문장
	End, // 대화화기 종료 시, '장소 선택'으로 넘어가기 전에 표시되는 문장
	Dialog, // 대화
	Selection, // 선택지,    
	Suggest, // 제시하기
	EventDialog,
	CompulsionSelection, // 강제적으로 유저에게 선택을 강요함
	InNote, // 노트에서 재생,
	ForcedEnd //NPC가 이동하여 강제로 대화가 종료됨.
}

public enum EventCategory
{
	Search = 11,
}

public enum GameEventType
{
	SystemMessage, // 시스템 메시지 출력
	AddEvidence, // 증거물 획득
	RemoveEvidence, // 증거물 삭제    
	AddNews, // 새 뉴스
			 /// <summary>
			 /// 새로운 지역(Place)를 추가해줍니다.
			 /// </summary>
	AddArea, //새로운 지역 지역 제거

	/// <summary>
	/// 지역(Place)를 제거합니다.
	/// </summary>
	RemoveArea, //지역 제거
	AddSelection, // 선택지 추가
	RemoveSelection, // 선택지 삭제
	AddNpc, //Npc 추가
	RemoveNpc, // Npc 삭제,
	ChangeNpcState, // NPC의 상태 변경
	AddSearch, // 장소에서 탐색 가능 여부 추가
			   // Search, // 장소에서 탐색함. 탐색 가능 여부가 있어야지만 탐색 가능,
	Dialog, // 다음 대화
	Case, // 사건 발생,
	GameOver,//, 
	AddNpcEvent,
	RemoveNpcEvent,
	AddCase,
	SwapEvidence,
	SetEvidence, // 원하는 장소에 증거물을 배치
	ShowCompulsion,
	ShowEnding,

	/// <summary>
	/// "Case" 트리거를 작동시킵니다.
	/// </summary>
	StartCase,
	/// <summary>
	/// 디텍티브 보드 아이템을 언락합니다.
	/// </summary>
	UnlockDT,
}

public enum NpcState
{
	Alive,
	Missing,
	Dead,
	Event0,
	Event1,
	Event2,
	Event3,
	Event4
}

public enum NpcRace
{
	백인 = 0, // 백인
	흑인 = 1, // 흑인
	황인 = 2, // 황인
	아랍 = 3, // 아랍
	라틴 = 4 // 라틴
}

public enum NpcSex
{
	Male,
	Female
}

public enum WorldEventType
{
	Dialog, // 대화
	AddArea, // 장소 오픈        
	Save
}

public enum StageTransformPosition
{
	Korea,
	USA,
	China
}

public enum RecordType
{
	Dialog,//대화
	Search//탐색 결과
}
/*
public enum CaseParameter
{
    CaseDate, // 사건이 일어날 날짜
    CaseIsOpen, // 게임 시작 시 사건이 발생했는지 여부 / true면 발생, false면 
    CaseLocation, // 사건이 벌어진 지역
    CaseAddress, // 사건이 벌어진 지역의 주소
    VictimName, // 피해자의 이름
    VictimSex, // 피해자의 성별
    VictimRace, // 피해자의 인종
    VictimAge, // 피해자의 나이
    VictimJob // 피해자의 직업

}*/

public enum CaseParameter
{
	CaseIndex, // 사건 번호
	CaseLocation, //사건이 벌어진 지역,
	CaseDate, // 사건이 일어날 날짜
	CaseTime, // 사건이 일어난 시간
	Event, // 이벤트 리스트
	Npc, // 영향을 받는 NPC 리스트
	Victim // 희생자

}

public enum EvidenceParameter
{
	CaseIndex,
	Date,
	Location
}

public enum StageType
{
	USA,
	China
}

public enum DialogTargetType
{
	Suspect,
	Colleague,
	Teacher,
	Police,
	Richard,
	Neighbor,
	Extra,
	End,
	Nothing
}

public enum PlaceType
{
	Default,
	Home,
	Company,
	Extra,
	Case,
	Road

}

public enum NpcReligion
{
	무교, // 무교
	기독교, // 기독교
	이슬람교, // 이슬람교 
	제로교, // 악마 숭배자 
	식인교// 식인 종교
}

/*
public enum BuildingType
{
    House,
    Store,
    Place,
    Neighbor,
    Extra,
    School
}*/

/// <summary>
/// /////////////////////////////////
/// </summary>
/// 

/*
public enum Personality
{
소시오패스 = 0,
사이코패스 = 1,
폭력적 = 2,
정서불안 = 3,
피해망상 = 4,
결벽증 = 5,
정신분열 = 6,
이기적 = 7
Sociopath = 0, // 소시오패스
Psychopath = 1, // 사이코패스
Violence = 2, // 폭력적인
Anxiety = 3, // 정서불안
PersecutoryDelusion = 4, // 피해망상
Mysophobia = 5, // 결벽증
Schizophrenia = 6, // 정신분열
Selfish = 7 // 이기적

}
*/
/*
public enum CriminalPersonality
{
    소시오패스 = 0,
    사이코패스 = 1
    //Sociopath = 0, // 소시오패스
    //Psychopath = 1 // 사이코패스
}*/

/*
public enum SubPersonality
{
외향적 = 0,
내성적 = 1,
매력적 = 2,
똑똑한 = 3,
오만한 = 4,
산만한 = 5,
충동적 = 6,
거짓말쟁이 = 7
/*Outgoing = 0, // 외향적인
Introspective = 1, // 내성적인
Attractive = 2, // 매력적인
Smart = 3, // 똑똑한
Proud = 4, // 오만한
Distracted = 5, // 산만한
Impulsive = 6, // 충동적
Lier = 7 // 거짓말쟁이
}
*/

/*
public enum CriminalAttribute
{
없음 = 0,
예비범죄자 = 1,
기타범죄 = 2,
조직적 = 3,
비조직적 = 4,
혼합형 = 5

None, //범죄가 없음,
ReserveCriminal, // 예비 범죄자. 비범인이지만 소시오패스나 사이코패스라서 곧 범죄를 저지를 가능성이 있음
ETC, //기타 범죄. 비범인 중에 살인외에 다른 범죄를 저지르고 있는 경우
Organized, // 조직적
Disorganized, // 비조직적,
Mixed // 혼합형
}
public enum CrimeCharacter
{
    RacHeadToBlack, // 흑인을 향한 인종 범죄
    RaceHeadToJaw, // 유대인을 향한 인종 범죄
    RaceHeadToArab, // 아랍인을 향한 인종 범죄
    SexContrary, // 반대되는 성을 향한 범죄
    SexHomosexuality, // 동성애를 향한 범죄
    FamilyParentsAbuse,// 부모 학대로 인한 범죄
    FamilyETCAbuse, // 부모 외 다른 가족의 학대로 인한 범죄
    FamilyFosterParentsAbuse, // 양부모 학대로 인한 범죄 
    NoneSexDrive, // 비성충동
    SexDrive // 성충동 
}
public enum CriminalCharacter
{
    없음 = 0,
    기타범죄 = 1,
    특정타겟살해 = 2,
    인종범죄 = 3,
    성범죄 = 4,
    쾌락살인 = 5,
    종교범죄 = 6,
    가족범죄 = 7,
    약물중독 = 8,
    술중독 = 9,
    재물범죄 = 10
    
    None, // 범죄를 저지를 생각이 없음. 비범인 전용
    ETC, //기타 범죄
    TargetFakeSerialKill, // 특정 타겟을 노리기 위한 위장 연쇄 살인
    Race,
    Sex,
    Family,
    Lust,
    Religion, // 종교
    LustMurder // 쾌락살인
               //Race, // 인종
               //Sex, // 성별
               //Family, // 가족 이력
               //Religion // 종교
}
public enum Sex
{
    남성, // 남성
    여성, // 여성
    MtoF, // 남자에서 여자로 바뀐 트랜스젠더
    FtoM // 여자에서 남자로 바뀐 트랜스젠더
}
public enum CharacterFace
{
    Body = 0, // 체형, 피부, 옷 등을 표현함
    Face = 1, // 이목구비와 상처, 점 등을 표현함
    Accessory = 2 // 머리, 안경, 귀걸이, 머리 장신구 등을 표현함
}
public enum CharacterFace
{
    Skin = 0, // 피부, 성별, 체형
    Outer = 1, // 겉옷
    Clothes = 2, // 옷 
    Neck = 3, // 목걸이 
    Hair = 4, // 헤어스타일
    FaceShape = 5, // 얼굴형
    Lip = 6, // 입술  이 부분에 이목구비가 들어 올 수 있음. 이목구비가 들어올 경우 입술, 눈썹, 점, 상처, 속눈썹, 수염 등이 사라짐. 
    Eyebrow = 7, // 눈썹
    Point = 8, // 점
    Scar = 9, // 상처
    Eyelash = 10, // 속눈썹
    Earing = 11, // 귀걸이
    Beard = 12, // 수염
    Glasses = 13, // 안경
    HairAccessory = 14 // 머리 장신구
}

public enum Race
{
    백인 = 0, // 백인
    흑인 = 1, // 흑인
    황인 = 2, // 황인
    아랍 = 3, // 아랍
    라틴 = 4 // 라틴
}
public enum EventTypeCode
{
    Case,
    News,
    Dialog
}
public enum VictimParameter
{
    Name,
    Sex,
    Race,
    Age
}
문제 없을 시 추후 삭제
public enum NPCName
{
    Richard, 
    Police,
    Teacher,
    Neighbor
}
public enum SuspectParameter
{
    Name, // 용의자의 이름
    Race, // 용의자의 인종
    Nation, // 용의자의 민족. 지금 버전에서는 하나로 통일할 예정
    Sex, // 용의자의 성별
    Religion, //용의자의 종교 int
    ReligionName,// 용의자의 종교 이름 string
    HomeArea, //용의자의 거주지역. Int
    HomeAddress, //용의자의 집 주소. Int
    HomeAreaName, //용의자의 거주지역 이름. String
    JobNumber, //용의자의 직업 번호. Int
    JobName, //용의자의 직업 이름. String
    CompanyNumber, //용의자의 직장 번호. Int
    CompanyArea, //용의자의 직장 지역 번호. Int
    CompanyName, //용의자의 직장 이름. String
    CompanyAreaName, //용의자의 직장이 있는 지역 이름. String
    Age, //용의자의 나이
    Height, //용의자의 키
    Face, // 용의자의 얼굴
    Family, // 용의자의 가족 이력
    Education,
    Wealth,
    School,
    Crime, //용의자의 
    Issue,
    Personality, //용의자의 주성격
    SubPersonality, //용의자의 보조성격
    CriminalAttribute, //용의자의 범죄 속성
    CriminalCharacter, //용의자의 범죄 성격
    IsAppear // 용의자가 유저에게 알려진 용의자인지 숨겨진 용의자인지. true면 알려진, false면 숨겨진
}
public enum Religion
{
    무교, // 무교
    기독교, // 기독교
    이슬람교, // 이슬람교 
    제로교, // 악마 숭배자 
    식인교// 식인 종교
}
public enum SceneType
{
    Title,
    Game
}
public enum NpcType
{
    Suspect,
    Person
}
*/



