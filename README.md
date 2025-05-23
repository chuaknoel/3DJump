# 🧗 점프킹 3D - Unity 미니 프로젝트

*Jump King* 스타일의 미니멀 3D 점프 게임.  
Unity를 활용해 캐릭터 조작, 상호작용, 아이템 시스템, 점프 구조물 등을 구현한 과제형 프로젝트입니다.

---

## 🎮 주요 기능

- ✅ **기본 이동 및 점프 (Rigidbody + Input System)**
- ✅ **체력/허기 UI (Image.fillAmount)**
- ✅ **조사 기능 (Raycast + UI)**
- ✅ **점프대 기능 (ForceMode.Impulse)**
- ✅ **아이템 데이터 관리 (ScriptableObject)**
- ✅ **아이템 효과 지속 (Coroutine 기반)**

---

## 🧱 프로젝트 구조

| 폴더 | 설명 |
|------|------|
| `Scripts/Player/` | 플레이어 이동, 상태, 상호작용 처리 |
| `Scripts/UI/` | 체력바, 인벤토리, 조사 UI |
| `Scripts/Item/` | 아이템 데이터 정의 및 사용 로직 |
| `Scripts/Environment/` | 점프대 및 점프대 자동 생성기 |
| `ScriptableObjects/` | `.asset` 형태로 저장된 아이템 데이터 |

---

## 🔨 사용 기술

- **Unity 2022.x**
- **C# (MonoBehaviour 기반)**
- **Unity Input System**
- **ScriptableObject**
- **Canvas + TextMeshPro 기반 UI 시스템**

---

## 🚀 실행 방법

1. `JumpTestScene` 또는 `MainScene`을 실행합니다.
2. `WASD`로 이동, `Space`로 점프합니다.
3. 아이템에 다가가서 `F`를 눌러 습득할 수 있습니다.
4. 점프대를 밟으면 위로 튕겨 올라갑니다.

---

## ⚙️ 점프대 생성 로직

> 점프대를 y축으로 일정 간격으로 5개 생성하며, x/z 방향으로는 소폭 랜덤 배치  
> `jumpForce = 200` 기준, 모든 점프대는 플레이어가 도달 가능한 위치에 생성됩니다.

```csharp
Vector3 offset = new Vector3(Random.Range(-2f, 2f), verticalSpacing * i, Random.Range(-2f, 2f));
Instantiate(jumpPadPrefab, start + offset, Quaternion.identity);
