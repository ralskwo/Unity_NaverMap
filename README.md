# 네이버 지도 API 유니티 프로젝트

---

## 프로젝트 개요

이 프로젝트는 유니티 6 버전을 사용하여 네이버 지도 API를 통합한 애플리케이션입니다. 프로젝트는 유니티 환경에서 지도 시각화 및 지오코딩 서비스를 제공하며, 주소 검색, 지도 표시, 확대/축소 기능과 더불어 **Dynamic Map API**를 활용하여 동적 마커 추가 및 상호작용 기능을 제공합니다.

---

## 주요 기능

### Static Map API 기능

- **지도 표시**: 네이버의 정적 지도 API를 사용하여 지도를 렌더링합니다.
- **지오코딩**: 주소를 지리적 좌표로 변환하여 지도에 표시합니다.
- **확대/축소 기능**: 마우스 스크롤(PC용) 및 핀치 제스처(모바일용)를 통한 지도 확대/축소를 지원합니다.
- **주소 검색**: 사용자가 주소를 검색하면 해당 위치로 지도가 자동 업데이트됩니다.

### Dynamic Map API 기능

- **동적 지도 렌더링**: 네이버 Dynamic Map API를 활용하여 실시간으로 상호작용 가능한 지도를 렌더링합니다.
- **동적 마커 추가**: Unity에서 전달받은 데이터를 기반으로 지도에 마커를 추가하고, 클릭 시 정보 창을 표시합니다.
- **마커 관리**:
  - 특정 마커를 선택적으로 삭제할 수 있습니다.
  - 지도에 있는 모든 마커를 한 번에 제거할 수 있습니다.
- **Unity와 JavaScript 통신**:
  - Unity의 WebView와 HTML/JavaScript 간의 실시간 데이터 동기화 및 명령 전달을 지원합니다.

---

## 프로젝트 구조

### 스크립트

#### Static Map API 관련

1. **NaverMapAPI.cs**

   - 네이버 지도 API와의 상호작용을 처리하며, API 자격 증명을 저장하고 객체가 씬 간에 파괴되지 않도록 싱글톤 패턴을 사용합니다.

2. **NaverMapController.cs**

   - 지도와의 상호작용을 관리하며, 주소 검색, 지도 드래그, 지도 이미지를 표시하는 기능을 담당합니다.
   - `IKeyPressButtonHandler` 인터페이스를 구현하여 키보드 입력(예: Enter 키로 검색)을 처리합니다.

3. **ZoomController.cs**

   - 마우스(PC용) 및 터치(모바일용) 확대/축소를 처리하는 기능을 제어합니다.
   - 싱글톤 패턴을 사용하여 전역 접근을 가능하게 합니다.

4. **GeocodeManager.cs**

   - 지오코딩 요청을 관리하며, 주소를 지리적 좌표로 변환합니다.
   - 비동기 작업을 구현하여 메인 스레드를 차단하지 않고 API 요청을 처리합니다.

5. **IKeyPressButtonHandler.cs**
   - 버튼 누름 이벤트를 처리하는 인터페이스를 정의하여, 다양한 컴포넌트가 일관되게 키 입력에 응답할 수 있도록 합니다.

#### Dynamic Map API 관련

6. **NaverMapWebView.cs**

   - Unity의 WebView를 통해 HTML 파일을 로드하고, 네이버 Dynamic Map API와 상호작용합니다.
   - Unity에서 동적으로 마커를 추가하거나 제거하는 명령을 전달합니다.

7. **DynamicMarkerManager.cs**
   - Unity와 JavaScript 간 통신을 통해 마커 추가, 삭제, 초기화 등의 기능을 제공합니다.
   - 사용자 입력에 따라 마커를 실시간으로 추가하고, 필요한 데이터를 WebView에 전달합니다.

---

## API 통합

### Static Map API

- **지오코딩**: `GeocodeManager.cs`의 `GetGeocode` 메서드를 사용하여 네이버 지오코드 API에 요청을 보내고, 응답을 처리하여 지도의 위치를 업데이트합니다.
- **정적 지도 표시**: `NaverMapController.cs`의 `GetMapTile` 메서드를 사용하여 네이버 정적 지도 API에 요청을 보내 정적 지도를 이미지로 렌더링합니다.

### Dynamic Map API

- **HTML 및 JavaScript 통합**:
  - 네이버 Dynamic Map API를 사용하는 HTML 파일이 Unity의 WebView를 통해 로드됩니다.
  - HTML 파일은 **개인 서버** 또는 **게시할 웹페이지**가 필요하지만, 기본적인 예제는 Unity 프로젝트의 `StreamingAssets` 폴더에 포함되어 있습니다.
- **동적 마커 추가**:
  - Unity에서 사용자 입력 데이터를 기반으로 JavaScript의 `addMarkerWithInfoWindow` 함수를 호출하여 마커를 추가합니다.
  - JavaScript는 마커를 지도에 표시하고, 클릭 시 정보 창을 띄우는 동작을 구현합니다.
- **마커 관리**:
  - 특정 마커를 제거하거나, 모든 마커를 초기화하는 JavaScript 함수(`removeMarker`, `removeAllMarkers`)를 호출하여 구현합니다.
- **Unity와 JavaScript 통신**:
  - Unity에서 JavaScript로 데이터를 전달하거나, JavaScript에서 Unity로 메시지를 반환하여 상호작용을 구현합니다.

---

## 설정 및 사용법

### Static Map 설정

1. **유니티 설정**:

   - 유니티 6 프리뷰 버전이 설치되어 있는지 확인합니다.
   - 프로젝트를 클론하거나 다운로드한 후 유니티에서 엽니다.

2. **API 자격 증명**:

   - `NaverMapAPI.cs` 파일에 있는 `clientID`와 `clientSecret`을 자신의 네이버 클라우드 플랫폼 자격 증명으로 업데이트합니다.

3. **프로젝트 실행**:
   - 유니티 에디터에서 `Play` 버튼을 눌러 애플리케이션을 실행합니다.
   - 입력 필드에 주소를 입력하고 `Enter` 키를 누르거나 검색 버튼을 클릭하여 지도를 확인합니다.
   - 마우스 스크롤이나 핀치 제스처를 사용하여 지도를 확대하거나 축소할 수 있습니다.

### Dynamic Map 설정

1. **WebView 설정**:

   - NHN GPM WebView 패키지를 프로젝트에 설치합니다.
   - `NaverMapWebView.cs`에서 HTML 파일 경로를 `StreamingAssets` 폴더에 있는 `naver_map` 파일을 참고하여 **개인 서버** 또는 **HTML 코드를 게시할 수 있는 웹 페이지**에 올려 사용합니다.
     - 기본 경로 예: `"https://example.com/naver_map.html"`

2. **Dynamic Map HTML 파일 구성**:

   - `StreamingAssets` 폴더에 포함된 예제 HTML 파일(`dynamicMap.html`)은 Dynamic Map API의 기본 기능(지도 표시, 마커 추가/삭제)을 포함하고 있습니다.
   - 필요에 따라 HTML 파일을 수정하여 프로젝트 요구사항에 맞게 확장할 수 있습니다.

3. **Unity 통합**:

   - Unity의 `NaverMapWebView` 스크립트를 통해 WebView를 초기화하고, Unity와 JavaScript 간 통신이 원활히 이루어지도록 설정합니다.

4. **프로젝트 실행**:
   - Unity에서 Play 버튼을 눌러 동적 지도와 마커 관리 기능을 확인합니다.
   - 주소 입력, 마커 추가 및 삭제, 확대/축소 기능을 테스트합니다.

---

## 의존성

- **Newtonsoft.Json**: 네이버 지오코드 API의 JSON 응답을 파싱하는 데 사용됩니다.
- **NHN GPM WebView**: Unity와 Dynamic Map API 통합을 위해 사용됩니다.

---

## 기능 설명

### 지도 시각화

- 정적 지도와 동적 지도를 모두 지원하며, Static Map API와 Dynamic Map API를 통합하여 구현했습니다.

### 동적 마커 추가 및 관리

- Unity에서 사용자가 입력한 데이터를 기반으로 JavaScript를 호출하여 마커를 추가합니다.
- 특정 마커를 제거하거나, 모든 마커를 초기화하는 관리 기능을 제공합니다.

### 지오코딩

- 주소를 입력받아 해당 위치의 좌표를 검색하고, 결과를 지도에 표시합니다.
