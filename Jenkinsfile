pipeline {
    agent any

      parameters {
        choice(name: 'ENVIRONMENT', choices: ['dev', 'qa', 'pre-prod'], description: 'Select Environment to deploy')
    }
    
    tools
    {
    git 'Default'
    }
    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main' , url : 'https://github.com/HKGandhi/InventoryApp.git' // Replace with your repo
            }
        }

        stage('Restore') {
            steps {
                bat 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                bat 'dotnet build --no-restore'
            }
        }

        stage('Test') {
            steps {
                bat 'dotnet test --no-build --verbosity normal --collect:"XPlat Code Coverage"'
                bat 'dotnet tool install --global dotnet-reportgenerator-globaltool'
                bat 'reportgenerator -reports:"**/TestResults/**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html'
            }
        }

 

        stage('Publish') {
            steps {
                bat 'dotnet publish -c Release -o out'
            }
        }

             stage('Deploy Based on Environment') {
            steps {
                script {
                    if (params.ENVIRONMENT == 'dev') {
                        echo "🚀 Deploying to DEV server..."
                        bat 'copy /Y out\\*.* C:\\Deployment\\Dev\\'
                    } else if (params.ENVIRONMENT == 'qa') {
                        echo "🚀 Deploying to QA server..."
                        bat 'copy /Y out\\*.* C:\\Deployment\\QA\\'
                    } else if (params.ENVIRONMENT == 'pre-prod') {
                        echo "🚀 Deploying to PRE-PROD server..."
                        bat 'copy /Y out\\*.* C:\\Deployment\\PreProd\\'
                    } else {
                        error "❌ Invalid environment selected."
                    }
                }
            }
        }
    }

    post {
         always {

           
                 publishHTML(target: [
                reportDir: 'coveragereport',
                reportFiles: 'index.html',
                reportName: 'Code Coverage Report'
            ])
            archiveArtifacts artifacts: 'out/**/*.*', fingerprint: true
        }
        success {
            echo 'Build and publish succeeded!'
        }
        failure {
            echo 'Build failed!'
        }
    }
}
