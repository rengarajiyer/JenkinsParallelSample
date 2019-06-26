def appName = "MSTest" // main application name
def buildNumber = env.BUILD_NUMBER           // jenkins build number - used to tag the docker image/container(s)


def Master_customeWorkspace = "C:/Testfolder/"
def Slave1_customWorkspace = "C:/Testfolder1/"
def Slave1_resultfile = "Slave1_Result.trx"
def Slave2_resultfile = "Slave2_Result.trx"
def Slave2_customWorkspace = "C:/Testfolder2/"
def ProjectBinary_Archive_file = "Testresult.zip"

def projectProperties = [
        buildDiscarder(logRotator(artifactDaysToKeepStr: '2', artifactNumToKeepStr: '2', daysToKeepStr: '2', numToKeepStr: '2')),
        disableConcurrentBuilds(),
        pipelineTriggers([pollSCM('H/10 * * * *')])
]
properties(projectProperties)					
// Artifactory server id configured in the jenkins along with credentials
artifactoryServer = Artifactory.server 'jenkins-artifactory-server-6.10.1'
def artifactoryPublishInfo

def Upload_ProjectArtifacts = """{
	"files": [
				{
					"pattern": "${Master_customeWorkspace}${ProjectBinary_Archive_file}",
					"target": "DemoProject/${buildNumber}/"
				}
			 ]
			}"""

def Upload_Slave1_TestArtifact = """{
	"files": [
				{
					"pattern": "${Slave1_customWorkspace}${Slave1_resultfile}",
					"target": "DemoProject/${buildNumber}/"
				}
			 ]
			}"""

def Upload_Slave2_TestArtifact = """{
	"files": [
				{
					"pattern": "${Slave2_customWorkspace}${Slave2_resultfile}",
					"target": "DemoProject/${buildNumber}/"
				}
			 ]
			}"""

def DownloadArtifactSlave1 = """{
	"files": [
				{
					"pattern": "DemoProject/${buildNumber}/",
					"target": "${Slave1_customWorkspace}"
				}
			 ]
			}"""

def DownloadArtifactSlave2 = """{
	"files": [
				{
					"pattern": "DemoProject/${buildNumber}/",
					"target": "${Slave2_customWorkspace}"
				}
			 ]
			}"""

def DownloadTestResultArtifact = """{
	"files": [
				{
					"pattern": "DemoProject/${buildNumber}/*.trx",
					"target": "${Master_customeWorkspace}"
				}
			 ]
			}"""

pipeline{
    agent none
    stages{
                stage('Checkout Source Code') {
                    agent { 
                        node{
                            label 'Master1' 
                            customWorkspace "${Master_customeWorkspace}"
                            
                        }
                    }
                    steps{
							cleanWs()
							checkout([$class: 'SubversionSCM', additionalCredentials: [], excludedCommitMessages: '', excludedRegions: '', excludedRevprop: '', excludedUsers: '', filterChangelog: false, ignoreDirPropChanges: false, includedRegions: '', locations: [[cancelProcessOnExternalsFail: true, credentialsId: 'c7ebd582-62ee-4b60-a6e5-c759d2fc4eef', depthOption: 'infinity', ignoreExternalsOption: true, local: '.', remote: 'https://svntvm.quest-global.com/svn/TSF/trunk/SE-Digitization_2018-19/POC/Trending_sampleproject']], quietOperation: true, workspaceUpdater: [$class: 'UpdateUpdater']])
						 }
                }
				stage('Compile Project') {
                    agent { 
                        node{
                            label 'Master1' 
                            customWorkspace "${Master_customeWorkspace}"
                            
                        }
                    }
                    steps{
							bat 'MSbuild Trending.sln'
						 }
                }
				stage('Archive Project Binaries') {
				
				agent { 
                        node{
                            label 'Master1' 
                            customWorkspace "${Master_customeWorkspace}"
                            
                        }
                    }
				
						// Arhive web tar to artifactory
					steps{	
					    bat "7z a -tzip ${ProjectBinary_Archive_file} * -xr!.svn/ -xr!*~"
						script
						{
						artifactoryPublishInfoService = artifactoryServer.upload(Upload_ProjectArtifacts)					
						artifactoryServer.publishBuildInfo(artifactoryPublishInfoService)
						}
						}
					  
					}   
					
			stage('Get Binaries on Slaves') {
              parallel {
                stage('Get Binaries Slave1') {
                    agent { 
                        node{
                            label 'Slave1' 
                            customWorkspace "${Slave1_customWorkspace}"
                        }
                    }
                    steps{
						cleanWs()
					   script
						{
						//Download artifact from Artifactory
						artifactoryPublishInfoService = artifactoryServer.download(DownloadArtifactSlave1)
						artifactoryServer.publishBuildInfo(artifactoryPublishInfoService)
						}
						//Extract the binaries
						bat "7z x ${Slave1_customWorkspace}${buildNumber}/${ProjectBinary_Archive_file} -o${Slave1_customWorkspace}"					
						 }
						 
                }
				stage('Get Binaries Slave2') {
                    agent { 
                        node{
                            label 'Slave2' 
                            customWorkspace "${Slave2_customWorkspace}"
							
                            
                        }
                    }
                    steps{
						cleanWs()
					  script
						{
						//Download artifact from Artifactory
						artifactoryPublishInfoService = artifactoryServer.download(DownloadArtifactSlave2)
						artifactoryServer.publishBuildInfo(artifactoryPublishInfoService)
						}		
					   //Extract the binaries
					   bat "7z x ${Slave2_customWorkspace}${buildNumber}/${ProjectBinary_Archive_file} -o${Slave2_customWorkspace}"						
							
						 }
                }              
                                
            }
			}
                                   
            stage('Testing') {
              parallel {
                stage('Testing on Slave1') {
                    agent { 
                        node{
                            label 'Slave1' 
                            customWorkspace "${Slave1_customWorkspace}"
                        }
                    }
                    steps{
					        bat "MSTest /testcontainer:${Slave1_customWorkspace}MSTest/bin/Debug/MSTest.dll /resultsfile:${Slave1_customWorkspace}${Slave1_resultfile}"
														
						 }

						
						 
                }
				stage('Testing on Slave2') {
                    agent { 
                        node{
                            label 'Slave2' 
                            customWorkspace "${Slave2_customWorkspace}"
                            
                        }
                    }
                    steps{
					
					        bat "MSTest /testcontainer:${Slave2_customWorkspace}MSTest/bin/Debug/MSTest.dll /resultsfile:${Slave2_customWorkspace}${Slave2_resultfile}"
							
						 }

						 
                }              
                                
            }
                        
        }
    }
	post {
			always {

				node('Slave1'){
						ws("${Slave1_customWorkspace}") {
														script
																{
																artifactoryPublishInfoService = artifactoryServer.upload(Upload_Slave1_TestArtifact)					
																artifactoryServer.publishBuildInfo(artifactoryPublishInfoService)
																}
														}
				}
				node('Slave2'){
						ws("${Slave2_customWorkspace}") {
														script
																{
																artifactoryPublishInfoService = artifactoryServer.upload(Upload_Slave2_TestArtifact)					
																artifactoryServer.publishBuildInfo(artifactoryPublishInfoService)
																}
														}
				}
			
			    node('Master1'){
						ws("${Master_customeWorkspace}") {
											script
												{
												//Download artifact from Artifactory
												artifactoryPublishInfoService = artifactoryServer.download(DownloadTestResultArtifact)
												artifactoryServer.publishBuildInfo(artifactoryPublishInfoService)
												}	

											bat "${Master_customeWorkspace}TrxfileMerge/TRX_Merger.exe /trx:${Master_customeWorkspace}${buildNumber}/Result/${Slave1_resultfile},${Master_customeWorkspace}${buildNumber}/Result/${Slave2_resultfile} /output:${Master_customeWorkspace}${buildNumber}/Result/Combined_Result.trx /report:${Master_customeWorkspace}Result/Test_Result.html"
											step([$class: 'MSTestPublisher', testResultsFile:"**/Combined_Result.trx", failOnError: true, keepLongStdio: true])
				  							//step([$class: 'MSTestPublisher', testResultsFile:"**/SampleResult.trx", failOnError: true, keepLongStdio: true])
				  //step([$class: 'Publisher', reportFilenamePattern: '**/test.xml'])
				  //junit '**/test.xml'
				  // rest of stage
				}
				
			    //step([$class: 'Publisher', reportFilenamePattern: '**/testng-results.xml'])
			    }
				
			}
		}
	
}
