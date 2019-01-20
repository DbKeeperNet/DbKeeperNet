pipeline {
  agent none

  triggers {
    pollSCM('H/10 * * * *')
  }

  options {
    skipDefaultCheckout true
  }

  environment {
    RELEASE_NUMBER = '3.0'
    VERSION_NUMBER = VersionNumber(versionNumberString: '3.0.${BUILDS_ALL_TIME}')
    SOLUTION = 'UnityInitializer.sln'
  }

  stages {
    stage('Compile release'){
      agent { label 'vs2017' }
      when { expression { env.TAG_NAME != null } }
      steps {
        script {
          currentBuild.displayName = "#" + "${env.TAG_NAME}".replace('build-', '')
        }
        
        echo 'Checkout'
            
        checkout scm

        echo 'Compiling'

        bat "\"${tool name: 'Default', type: 'msbuild'}\\msbuild.exe\" \"build.msbuild\" /p:BuildNumber=${TAG_VERSION_NUMBER}"

        dir ('bin') {
              stash 'build'
        }
        dir ('packages\\NUnit.ConsoleRunner.3.9.0\\tools') {
              stash 'nunit'
        }
      }
    }
    stage('Compile dev'){
      agent { label 'vs2017' }

      when { expression { env.TAG_NAME == null } }
      steps {
        script {
          currentBuild.displayName = "#${VERSION_NUMBER}"
        }
        
        echo 'Checkout'
            
        checkout scm

        echo 'Compiling'

        bat "\"${tool name: 'Default', type: 'msbuild'}\\msbuild.exe\" \"build.msbuild\" /p:BuildNumber=${VERSION_NUMBER}"

        dir ('bin') {
              stash 'build'
        }
        dir ('packages\\NUnit.ConsoleRunner.3.9.0\\tools') {
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
            bat "nunit\\nunit3-console.exe --workers=1 bin\\DbKeeperNet.Extensions.SQLite.Tests.dll bin\\DbKeeperNet.Engine.Tests.Full.dll"
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
            bat "nunit\\nunit3-console.exe --workers=1 bin\\DbKeeperNet.Extensions.SqlServer.Tests.dll bin\\DbKeeperNet.Extensions.AspNetRolesAndMembership.Tests.dll"
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
            bat "nunit\\nunit3-console.exe --workers=1 bin\\DbKeeperNet.Extensions.Pgsql.Tests.dll"
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
            bat "nunit\\nunit3-console.exe --workers=1 bin\\DbKeeperNet.Extensions.Firebird.Tests.dll"
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
            bat "nunit\\nunit3-console.exe --workers=1 bin\\DbKeeperNet.Extensions.Mysql.Tests.dll"
          }
          post {
            always {
              nunit testResultsPattern: 'TestResult.xml'
            }
          }
        }
      }
    }

    stage('Publish nugets') {
        agent { label 'vs2017' }

        when { tag 'build-*' }
        steps {
          dir ('bin') {
            unstash 'build'
          }

          archiveArtifacts artifacts: 'bin\\*.nupkg', onlyIfSuccessful: true

          withCredentials([string(credentialsId: 'nuget', variable: 'NUGET_API_KEY')]) {
//            bat "nuget push bin\\**.nupkg ${NUGET_API_KEY} -source https://api.nuget.org/v3/index.json"
          }
        }
    }
  }
  
}
