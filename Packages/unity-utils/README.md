# unity-utils

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![CodeFactor](https://www.codefactor.io/repository/github/unity-korea-community/unity-utils/badge)](https://www.codefactor.io/repository/github/unity-korea-community/unity-utils)
[![CodacyBadge](https://api.codacy.com/project/badge/Grade/7edcab32b58346089dc9fd84caff2bd8)](https://app.codacy.com/gh/unity-korea-community/unity-utils?utm_source=github.com&utm_medium=referral&utm_content=unity-korea-community/unity-utils&utm_campaign=Badge_Grade_Settings)
![release](https://img.shields.io/github/v/release/unity-korea-community/unity-utils)
[![Discord](https://img.shields.io/discord/591914197219016707.svg?label=&logo=discord&logoColor=ffffff&color=7389D8&labelColor=6A7EC2)](https://discord.gg/FKXA2yzR)

### CI Status

[![CI_SONAR_CLOUD](https://github.com/unity-korea-community/unity-utils/actions/workflows/sonarcloud-analysis.yml/badge.svg?branch=workspace)](https://github.com/unity-korea-community/unity-utils/actions/workflows/sonarcloud-analysis.yml)
[![CI_Documentation](https://github.com/unity-korea-community/unity-utils/actions/workflows/documentation.yml/badge.svg)](https://github.com/unity-korea-community/unity-utils/actions/workflows/documentation.yml)

## 소개

다른 패키지에서 간단하게 사용하는 코드 라이브러리 모음입니다.

풀링, Extension Method 모음 등이 있습니다.

## 기능

- SimplePool&lt;T&gt; - 간단하게 사용하는 제네릭 오브젝트 풀 클래스입니다.
  - 바로 써도 되고, 상속해서 `OnSpawn`, `OnDeSpawn`등을 override해서 사용할 수 있습니다.
  - 테스트 코드: [https://github.com/unity-korea-community/unity-utils/blob/master/Tests/Runtime/SimplePoolTests.cs](https://github.com/unity-korea-community/unity-utils/blob/master/Tests/Runtime/SimplePoolTests.cs)
- DataSender&lt;T&gt; - IObservable&lt;T&gt;, IDisposable
  - 옵저버 클래스입니다.
  - 테스트 코드: [https://github.com/unity-korea-community/unity-utils/blob/master/Tests/Runtime/DataSenderTests.cs](https://github.com/unity-korea-community/unity-utils/blob/master/Tests/Runtime/DataSenderTests.cs)
- Extensions
  - Collection Extension
    - `ToStringCollection()`, `Foreach()`, `Dequeue()`, `Pop()` 등 지원
    - 테스트 코드: [https://github.com/unity-korea-community/unity-utils/blob/master/Tests/Runtime/CollectionExtensionTests.cs](https://github.com/unity-korea-community/unity-utils/blob/master/Tests/Runtime/CollectionExtensionTests.cs)
  - Random Extension
    - IEnumerable&lt;T&gt;.`Random()`, List.`Shuffle()`등 지원
    - 테스트 코드: [https://github.com/unity-korea-community/unity-utils/blob/master/Tests/Runtime/RandomExtensionTests.cs](https://github.com/unity-korea-community/unity-utils/blob/master/Tests/Runtime/RandomExtensionTests.cs)

## 설치

Unity Editor/상단 Window 탭/Package Manager/+ 버튼/‌

Add package from git URL 클릭 후‌

이 저장소의 URL 입력‌

​[https://github.com/unity-korea-community/unity-utils.git](https://github.com/unity-korea-community/unity-utils.git)
