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
