## [3.2.1](https://github.com/Elders/Pandora.Consul/compare/v3.2.0...v3.2.1) (2022-08-16)


### Bug Fixes

* pipeline update ([7908ef5](https://github.com/Elders/Pandora.Consul/commit/7908ef5ec5d96bb4a65e4b64e4f2c6cfce33f065))

# [3.2.0](https://github.com/Elders/Pandora.Consul/compare/v3.1.1...v3.2.0) (2022-07-06)


### Features

* Updates packages ([1c937fc](https://github.com/Elders/Pandora.Consul/commit/1c937fc12713f8d16128f2c231748e9bee55daf9))

## [3.1.1](https://github.com/Elders/Pandora.Consul/compare/v3.1.0...v3.1.1) (2022-04-23)


### Bug Fixes

* Fixes reload bug with OptionsMonitor ([361315d](https://github.com/Elders/Pandora.Consul/commit/361315dd079ae1342b4eee0a83457694282cfb52))

# [3.1.0](https://github.com/Elders/Pandora.Consul/compare/v3.0.1...v3.1.0) (2022-03-23)


### Features

* Properly implements the consul reload functionality ([10fc295](https://github.com/Elders/Pandora.Consul/commit/10fc295a91986fe977ae29ba14f98f52b957fd0e))

## [3.0.1](https://github.com/Elders/Pandora.Consul/compare/v3.0.0...v3.0.1) (2021-12-13)


### Bug Fixes

* Adds lastIndex to request for GetAll ([e021e64](https://github.com/Elders/Pandora.Consul/commit/e021e640cc78c216888762df9692290bfcaea596))

# [3.0.0](https://github.com/Elders/Pandora.Consul/compare/v2.0.0...v3.0.0) (2021-12-13)

#### 2.0.0 - 26.03.2020
* Consul keys are case sensitive so we make everything lower
* Upgrades to netcore3.1
* Getting configurations is not bounded to the PandoraContext

#### 1.1.0 - 10.12.2018
* Updates to DNC 2.2

#### 1.0.1 - 15.11.2018
* Skips non pandora keys registered in consul

#### 1.0.0 - 15.11.2018
* Targets .netcore 2.1
* Adds PandoraConsulConfigurationSource

#### 0.8.0 - 20.06.2016
* Updates to latest Pandora. Fixed configuration merging

#### 0.7.0 - 01.06.2016
* Use consul formating for key

#### 0.6.2 - 27.11.2016
* Does not return values which are null when GetAll(...) is executed

#### 0.6.1 - 22.11.2016
* New logo

#### 0.6.0 - 15.11.2016
* Implemented GetAll method

#### 0.5.3 - 03.11.2016
* Remove the check for empty value when inserting a key/value

#### 0.5.2 - 03.11.2016
* Adds Exists(...) method for PandoraForConsul

#### 0.5.1 - 01.11.2016
* Properly builds the consul client

#### 0.5.0 - 01.11.2016
* Adds ConsulForPandora configuration repository
