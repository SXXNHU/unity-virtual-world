# 가상세계 (Virtual World)

2025년 1학기 **가상세계** 수업에서 Unity Engine으로 진행한 실습과 과제를 모아 둔 저장소입니다.
각 폴더는 독립된 Unity 프로젝트이며, 모든 스크립트는 수업 실습 중 직접 작성했습니다.

## 저장소 구조

```
.
├── Assignment/              # 프로그래밍 과제
│   ├── Assignment1/         # 과제 1: 도로 메쉬와 웨이포인트 주행
│   ├── Assignment2/         # 과제 2: 동물 먹이 주기 게임
│   ├── Assignment3/         # 과제 3: 격자 기반 길찾기와 베지어 이동
│   └── VW_Programming_Assignment_{1,2}.pdf   # 과제 안내문
└── Create-With-Code/        # Unity Learn "Create with Code" 따라하기
    ├── Prototype1 ~ Prototype5
    └── Prototype6           # 우선순위 큐 실습
```

각 프로젝트의 `Assets/Scripts`에 직접 작성한 C# 코드가 있습니다. 과제 폴더에는 제출한 `report.pdf`도 함께 있습니다.

## 과제 (Assignment)

| 프로젝트 | 내용 | 주요 스크립트 |
| --- | --- | --- |
| Assignment1 | 곡선 도로를 메쉬로 생성하고, 웨이포인트를 따라 차량이 주행합니다. 카메라 전환과 리스폰도 구현했습니다. | `BaseMakeMesh`, `CurvedRoadLeft/Right`, `WaypointGenerator`, `Left/RightWaypointFollower`, `CameraSwitcher`, `Respawn` |
| Assignment2 | 플레이어가 동물에게 먹이를 던져 허기 게이지를 채우는 게임입니다. 동물과 장애물을 스폰하고, 미니맵을 제공합니다. | `Animal`, `AnimalSpawner`, `FeedController`, `FoodProjectile`, `HungerBar`, `ObstacleSpawner`, `MiniMapCamera` |
| Assignment3 | Assignment2에 격자 맵을 더했습니다. 동물이 BFS, Greedy, A\* 중 선택한 알고리즘으로 길을 찾고, 베지어 곡선으로 이동할 수 있습니다. | `GridManager`, `GridCell`, `PathfindingManager`, `PriorityQueue`, `AnimalAI`, `BezierMover`, `UIManager` |

Assignment3에서는 숫자 키 `1`, `2`, `3`으로 길찾기 알고리즘을 바꾸고(BFS, Greedy, A\*), `B` 키로 베지어 이동을 시작하거나 멈춥니다.

## Create with Code

| 프로젝트 | 내용 | 주요 스크립트 |
| --- | --- | --- |
| Prototype1 | 장애물을 피해 차량을 운전하는 기본 프로토타입 | `PlayerController`, `FollowPlayer` |
| Prototype2 | 동물에게 먹이를 던지는 게임 | `PlayerController`, `SponeManager`, `MoveForward`, `DetectCollision`, `DestroyOutOfBounds` |
| Prototype3 | 점프하며 장애물을 넘는 러너. 배경이 반복됩니다. | `PlayerController`, `SpawnManager`, `MoveLeft`, `RepeatBackground` |
| Prototype4 | 적을 밀어 떨어뜨리는 게임 | `PlayerController`, `Enemy`, `SpawnManager`, `RotateCamera` |
| Prototype5 | 마우스로 오브젝트를 클릭하는 게임. 난이도를 고를 수 있습니다. | `GameManager`, `Target`, `Difficulty`, `DestroyAfterDelay` |
| Prototype6 | 우선순위 큐 실습 | `PriorityQueue` |

## 실행 방법

1. [Unity Hub](https://unity.com/download)를 설치합니다.
2. 아래 표의 에디터 버전을 설치합니다.
3. Unity Hub에서 **Add → Add project from disk**로 원하는 프로젝트 폴더를 엽니다.
4. `Assets/Scenes`의 씬을 열고 Play를 누릅니다.

| 에디터 버전 | 프로젝트 |
| --- | --- |
| Unity 6000.0.42f1 | Assignment1, Assignment2, Prototype1 ~ 4 |
| Unity 2021.3.45f1 | Assignment3, Prototype5, Prototype6 |

처음 프로젝트를 열면 Unity가 `Library` 폴더를 자동으로 생성하므로 시간이 조금 걸립니다.

## 참고

- `Library`, `Logs`, `obj`, `UserSettings` 같은 Unity 자동 생성 폴더와 `.csproj`, `.sln`, `.unitypackage`는 `.gitignore`로 제외했습니다.
- `Assets/Course Library` 등의 모델, 텍스처, 프리팹은 Unity Learn에서 제공하는 교육용 에셋입니다. 직접 만든 것이 아닙니다.
- 이전 커밋에 올라가 있던 자동 생성 파일은 저장소에서 제거했습니다. 과거 커밋 기록에는 남아 있습니다.
