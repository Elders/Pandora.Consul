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
