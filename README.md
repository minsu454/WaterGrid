# 💧 Water Grid

## 🎮 게임 소개

**Water Grid**는 간단한 리소스와 터치만으로 조작 가능한 **2D 퍼즐 게임**입니다. 제한된 자원으로 최대한의 공급 효율을 만들어내는 전략적 연결 퍼즐로, **Mini Metro**에서 영감을 받아 제작 중입니다.

플레이어는 **물 저장소에서 출발하여 펌프를 설치하고**, 멀리 위치한 가정집들에 물을 공급해야 합니다. 제한된 **공급 범위**, **연결 수** 등의 조건을 고려해 **최적의 구조를 설계**하고, 각 스테이지를 클리어하며 **높은 점수와 공급률을 달성**하는 것이 핵심입니다.

---

## ⚙️ 기술 스택

* **Unity**: 2022.3.60f1
* **C#**: 8.0

---

## 🚀 키워드 요약

* 🔹 간단한 리소스와 터치만으로 조작 가능한 2D 퍼즐
* 🔹 제한된 공급 범위와 연결 수로 최적의 설치 위치와 구조 설계 필요
* 🔹 층/라운드 단위로 구성된 스테이지, 공급률에 따라 다음 단계 진입
* 🔹 펌프 업그레이드, 커버 범위 확장 등 장기 플레이 유도 요소 탑재 예정

---

## 🎲 게임 시스템

Water Grid는 **시간에 따라 변화하는 수요**와 **제한된 자원**을 전략적으로 연결해가는 **흐름 기반 퍼즐 게임**입니다.

* 💧 물 저장소에서 출발하여 가정집에 물 공급
* ⚙️ 펌프의 설치 위치, 공급 범위, 연결 수 제한 존재
* 🔗 연결 구조는 시각적으로 표시되어 빠른 전략 판단 가능
* 🗺️ 스테이지마다 지형/조건이 다르고 새로운 전략 요구
* 🎯 무작위보다는 퍼즐 중심, 반복 가능한 정적 구조

---

## 🧠 프로젝트 목표

* UI 조작만으로도 전략적 재미를 주는 퍼즐 구현
* 시각화와 제약 조건을 통한 구조적 설계
* 리플레이성 강한 고정 스테이지 구조 구현

---

## 🧩 구현한 핵심 기능

### 🔗 연결 제어 시스템 (LineRenderer 기반)

연결 관리 전반을 단일 NodeManager에서 담당하며, 선 생성/삭제/연결/해제까지의 흐름을 모두 관리하는 구조.

* 🎯 실시간 라인 드래그 및 시각 피드백 처리
* ✅ 라인 생성 전 조건 검사 (거리, 중복 연결, 타입 불일치 등)
* 🔄 상호 연결 구조 유지 (부모/자식) 및 상태 반영

### ✨ 구조적 특징 및 장점

* 노드 연결 정보를 딕셔너리 구조로 보존하여 빠른 탐색
* 노드 타입, 반경, 연결 제한 등 복합 조건에 따른 유효성 검사
* 인터페이스 기반의 입력 처리로 사용자 조작 흐름과 연동
* 연결 취소/재연결 등 플레이어 인터랙션 중심 흐름 설계

---

### ⏱️ TimeScale 직접 관리 시스템 (Custom Time Manager)

기반의 멀티 타임스케일 제어 기능을 통해, 게임 내 상황별 시간 흐름을 독립적으로 제어할 수 있도록 구현.

* 🔄 상황에 따라 정밀하게 시간 흐름 제어 (정지, 느리게, 빠르게 등)
* 📦 Enum 기반 컨테이너로 관리 및 확장성 고려
* 🔧 직접 구현한 시간계산을 통해 UI, 애니메이션, 게임 로직과의 동기화 지원

```csharp
public static class TimeManager
{
    private static readonly Dictionary<TimeType, float> _timeContainerDict = new();

    public static void Init()
    {
        foreach (TimeType type in Enum.GetValues(typeof(TimeType)))
            _timeContainerDict.Add(type, 1f);
    }

    public static void SetTime(this TimeType type, float timeScale)
    {
        _timeContainerDict[type] = timeScale;
    }

    public static float Get(this TimeType type) => _timeContainerDict[type];
}
```

#### ✅ 사용 예시

```csharp
public void OnPause() => TimeType.InGame.SetTime(0f);
public void OnPlay() => TimeType.InGame.SetTime(1f);

private void Update()
{
    curCount += TimeType.InGame.Get() * Time.deltaTime;
    if (curCount >= maxCount)
    {
        TimeType.InGame.SetTime(0f);
        GameOver();
    }
    SetFillAmount(curCount / maxCount);
}
```

### ✨ 구조적 특징 및 장점

* 기존 `Time.timeScale` 대비 모듈화된 다중 시간 관리 구조 설계
* 게임 흐름, 애니메이션, UI 등 다양한 시간 의존 요소와의 연계 가능
* 전역 시간 제어의 충돌 없이 개별 타임라인 분리 가능
* Enum 중심 컨테이너 기반으로 유지보수성과 확장성 확보

---

### 🎞️ 커스텀 Tween 시스템 (DotweenEx)

DOTween의 핵심 로직을 재현한 내부 Tween 유틸리티를 구현하여, 경량화된 반복 연출이나 UI 애니메이션에 사용.

* 📈 값 보간 및 시간 기반 애니메이션 처리
* 🔁 Yoyo, None 등 간단한 LoopType 지원
* ⏱️ TimeType 기반의 일관된 시간 흐름과 연계

```csharp
public sealed class DotweenEx
{
    float startValue, endValue, duration, value;
    float curTime; bool loop; LoopType type;
    Action deleted, callback;

    public DotweenEx(float start, float dur, float end, Action del)
    {
        startValue = start; duration = dur; endValue = end; deleted = del;
    }

    public void OnUpdate()
    {
        curTime += TimeType.InGame.Get() * Time.deltaTime;
        float t = Mathf.Clamp01(curTime / duration);
        value = Mathf.Lerp(startValue, endValue, t);
        if (t == 1f) OnCompleted();
    }

    private void OnCompleted()
    {
        callback?.Invoke();
        if (loop)
        {
            if (type == LoopType.Yoyo) (endValue, startValue) = (startValue, value);
            curTime = 0f;
        }
        else deleted.Invoke();
    }

    public DotweenEx SetLoop(LoopType loopType = LoopType.None) { loop = true; type = loopType; return this; }
    public DotweenEx OnCompleted(Action callback) { this.callback = callback; return this; }
}
```

#### ✅ 사용 예시

```csharp
void Init()
{
    dotween = new DotweenEx(0, 1.5f, 1, () => dotween = null).SetLoop(LoopType.Yoyo);
}

void Update()
{
    dotween.OnUpdate();
    _warningContainer.SetOutLines(dotween.Value);
}
```

### ✨ 구조적 특징 및 장점

* DOTween과 유사한 동작 구조를 자체 구현해 외부 의존도 제거
* `TimeType`과 결합되어 일시정지/가속 등 전체 흐름과 일관성 유지
* 간결한 API 설계로 다양한 연출(점멸, 펄스, 슬라이드 등)에 확장 적용 가능
* 삭제 콜백 처리로 메모리 누수 없이 반복 제어 가능

---

### 🎯 하이브리드 확률 분포 시스템 (Area-Group 기반 Weighted Random)

에디터에서 사용자가 정의한 Area 그룹 단위의 가중치를 기준으로 분포를 설정하고, 내부 타일은 자동으로 균등 분배되도록 설계한 구조.

> 일반적인 무작위 배치는 새로운 타일이 추가될 때마다 전체 확률의 비가 변동되는 단점이 있음. 이 시스템은 **고정된 그룹 확률을 유지하면서도 내부 타일을 유동적으로 관리**하기 위한 방식.

* 🎲 AreaData 단위로 전체 맵 구역을 나누고, 각 구역마다 독립된 Weight 값을 설정
* 🧠 전체 Weight 비율을 기준으로 자동 비율 조정 → 사용자 의도에 맞는 확률 분포 구현
* ⚖️ 같은 Area 내부에서는 균등 확률 적용 → 일관된 결과 유지
* 🔧 타일이 추가되거나 제거되어도 전체 Weight 구조에는 영향 없음

#### ✨ 구조적 특징 및 장점

* 확률 기반 랜덤 배치 시스템의 대표적인 문제점인 비율 깨짐을 회피
* 그룹 단위 제어로 고난이도 레벨 디자인에서의 통제력 향상
* `Area` 와 `TileType`을 분리하여 재사용성과 확장성 확보
* 확률 테스트 결과를 Scene 뷰 상 시각화(Label, Color)하여 디버깅과 밸런싱 편의성 극대화

---

### 🗺️ 맵 에디터 기반 맵 관리 시스템 (Hexagonal Editor 기반)

Hex 좌표계를 기반으로 한 SceneView 기반 에디터 도구를 제작하여, 개발 중이던 스테이지 맵을 시각적으로 직접 제작/편집할 수 있도록 제작하여, 개발 중이던 스테이지 맵을 시각적으로 직접 제작/편집할 수 있도록 구현.

* 🧱 육각형 그리드 기반 커스텀 타일 배치 시스템 구현
* 🎨 Area, TileType 브러시 및 확률 기반 구역 분포 도구 제공
* 📦 JSON 저장/로드 기능을 통해 맵 데이터를 직렬화하여 반복 재사용 가능
* 🎯 유니티 Scene 뷰 상에서 직접 헥사곤 타일 및 시각 연출 확인 가능

```csharp
[MenuItem("Tools/MapEditor/Create")]
static void Create()
{
    CreateComstomWindow("Hex Map Editor", new Vector2(550f, 400f), new Vector2(550f, 400f));
}

private void DrawHexToWorld(Vector3 screenPos)
{
    FindHexagon(screenPos, (posInt) =>
    {
        if (_hexDict.TryGetValue(posInt, out var value) is false)
            _hexDict[posInt] = new(selectedAreaIndex, TileType.None);
    });
}

private void SaveData(string path)
{
    List<TileData> tileDataList = new();
    foreach (var pair in _hexDict)
        tileDataList.Add(new TileData(pair));
    SaveManager.Save(path, JsonUtility.ToJson(...));
}
```

### ✨ 구조적 특징 및 장점

* 타일의 시각적 배치와 내부 데이터(JSON) 분리, 저장 시 직렬화 구조 반영

* SceneView 내 실시간 헥사 타일 렌더링 (Handles, DrawPolyLine, 퍼센트 라벨 등)

* 드래그 기반 직관적 에디팅 UX 구현 (Area, Brush, Erase 툴 분기 처리)

* 확률 기반 AreaData.Weight 시스템을 통한 구역별 랜덤 분포 시뮬레이션 가능

* TileType에 따라 인터랙션 부여 가능 (예: House 타일 개수 설정, Pump 제한 등)

---

* 📄 [Figma 기획서 보기](https://www.figma.com/board/lwbRaSoubcDmvwPYx1NySk/WaterGrid?node-id=46-38&t=CZ49fRRJ9Re4rsr6-0)

---

## 👤 제작자

* 1인 개발 프로젝트 / Unity 프로그래머

---

> 🔄 이 README는 구현 상황에 따라 계속 업데이트될 예정입니다.
