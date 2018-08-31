pipeline {
  agent none

  triggers {
    pollSCM('H/10 * * * *')
  }

  environment {
    RELEASE_NUMBER = '3.0'
    VERSION_NUMBER = VersionNumber(versionNumberString: '3.0.${BUILDS_ALL_TIME}')
    SOLUTION = 'UnityInitializer.sln'
  }

  stages {
    stage('Checkout'){
      agent { label 'vs2017' }

      steps {
        script {
          currentBuild.displayName = "#${VERSION_NUMBER}"
        }

        cleanWs()

        checkout scm
      }
    }
    stage('Compile'){
      agent { label 'vs2017' }

      steps {
        echo 'Compiling'

        bat "\"${tool name: 'Default', type: 'msbuild'}\\msbuild.exe\" \"build.msbuild\" /p:BuildNumber=${VERSION_NUMBER}"

        dir ('bin') {
              stash 'build'
        }
        dir ('packages\\NUnit.ConsoleRunner.3.8.0\\tools') {
              stash 'nunit'
        }
      }
    }
    stage('Unit tests') {
      parallel {
        stage('SQLite') {
          agent { label 'vs2017' }
          steps{
            dir ('bin') {
              unstash 'build'
            }
            dir ('nunit') {
              unstash 'nunit'
            }
            bat "nunit\\nunit3-console.exe bin\\DbKeeperNet.Extensions.SQLite.Tests.dll bin\\DbKeeperNet.Engine.Tests.Full.dll"
          }
          post {
            always {
              nunit testResultsPattern: 'TestResult.xml'
            }
          }
        }
        stage('SQL Express') {
          agent { label 'sqlexpress' }
          steps{
            dir ('bin') {
              unstash 'build'
            }
            dir ('nunit') {
              unstash 'nunit'
            }
            bat "nunit\\nunit3-console.exe bin\\DbKeeperNet.Extensions.SqlServer.Tests.dll bin\\DbKeeperNet.Extensions.AspNetRolesAndMembership.Tests.dll"
          }
          post {
            always {
              nunit testResultsPattern: 'TestResult.xml'
            }
          }
        }
        stage('PostgreSql') {
          agent { label 'postgresql' }
          steps{
            dir ('bin') {
              unstash 'build'
            }
            dir ('nunit') {
              unstash 'nunit'
            }
            bat "nunit\\nunit3-console.exe bin\\DbKeeperNet.Extensions.Pgsql.Tests.dll"
          }
          post {
            always {
              nunit testResultsPattern: 'TestResult.xml'
            }
          }
        }
        stage('Firebird') {
          agent { label 'firebird' }
          steps{
            dir ('bin') {
              unstash 'build'
            }
            dir ('nunit') {
              unstash 'nunit'
            }
            bat "nunit\\nunit3-console.exe bin\\DbKeeperNet.Extensions.Firebird.Tests.dll"
          }
          post {
            always {
              nunit testResultsPattern: 'TestResult.xml'
            }
          }
        }
        stage('MySql') {
          agent { label 'mysql' }
          steps{
            dir ('bin') {
              unstash 'build'
            }
            dir ('nunit') {
              unstash 'nunit'
            }
            bat "nunit\\nunit3-console.exe bin\\DbKeeperNet.Extensions.Mysql.Tests.dll"
          }
          post {
            always {
              nunit testResultsPattern: 'TestResult.xml'
            }
          }
        }
      }
    }
    stage('Artifacts') {
        agent { label 'vs2017' }

        when { expression { env.CHANGE_ID == null } }
        steps {
            dir ('bin') {
            unstash 'build'
            }

            archiveArtifacts artifacts: 'bin\\**.nupkg', onlyIfSuccessful: true
        }
    }
  }
  
}
