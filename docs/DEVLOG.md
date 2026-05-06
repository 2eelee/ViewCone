## 2026.05.02

### 한 것
- Unity 6000.4.5f1 LTS + URP 템플릿으로 프로젝트 생성
- Git 리포 연결, GitHub에 푸시 (2eelee/ViewCone)
- 폴더 구조 정리 (_Project/Scripts/{AI, Player, Utils, Debug}, Prefabs, Scenes, Art)
- 문서 4종 빈 파일 생성 (README, ARCHITECTURE, OPTIMIZATION, DEVLOG)
- Android Build Support 모듈 설치, SDK 셋업
- 갤럭시 S22 USB 디버깅 연결, adb 인식 확인
- 빈 씬 안드로이드 빌드 → 실기기 실행 성공

### 막힌 것 / 트러블슈팅
- **Unity 첫 실행 시 패키지 권한 에러 (EPERM)**: Unity Hub 관리자 권한 실행으로 해결
- **GitHub 푸시 거부 (100MB 초과)**: .gitignore에 Library 폴더 누락. `git rm -r --cached`로 트래킹 제거 후 재푸시
- **adb devices에 S22 미인식**: PC 앞면 USB 포트 데이터 전송 미지원. 뒷면 포트로 변경 후 해결
- **빌드 시 Win32 IO error 112**: C드라이브 용량 부족. 공간 확보 후 재빌드 성공

### 배운 것
- Unity 프로젝트의 Library 폴더는 자동 생성되므로 git 트래킹 제외 필수
- Android 빌드는 임시 파일 + Gradle 캐시로 디스크 공간 여유분 필요
- USB 케이블/포트가 데이터 전송 지원 여부에 따라 adb 인식 좌우됨

---

## 2026.05.03

### 한 것
- 가드 순찰 시스템 (NavMeshAgent + Waypoint)
- 시야 콘 구현 (FieldOfView + 메시 시각화)
- 상태머신 (Patrol/Suspicious/Alert/Chase/Search)
- 시야 콘 색상 변화 (흰→노랑→주황→빨강)
- 머리 위 상태 아이콘 (?, !, !!, ?!)
- 의심도 슬라이더 UI
- 수색 중 좌우 둘러보기
- 다중 가드 협동 (Chase 시 주변 가드 Alert)
- 청각 시스템 (Shift 달리기 시 소리 반경 발생)
- 플레이어 마우스 회전 + Shift 달리기

### 막힌 것 / 트러블슈팅
- **Mixamo 애니메이션 슬라이딩**: Loop Time + In Place로 해결
- **NavMeshAgent 순간이동**: SamplePosition으로 웨이포인트 NavMesh 위치 보정
- **다중 가드 협동 미감지**: Capsule Collider + Guard 레이어 추가로 해결

### 배운 것
- Mixamo 애니메이션은 In Place로 받아야 루트 모션 문제 없음
- NavMesh.SamplePosition으로 웨이포인트 위치 보정 필요
- 상태머신 + BT 하이브리드 구조가 게임 AI 설계의 실제 패턴
