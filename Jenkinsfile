pipeline {
    agent any
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
            }
        }

        stage('Publish') {
            steps {
                bat 'dotnet publish -c Release -o out'
            }
        }
    }

    post {
         always {
            publishCoverage adapters: [
                coberturaAdapter('**/TestResults/**/coverage.cobertura.xml')
            ],
            sourceFileResolver: sourceFiles('NEVER_STORE')
        }
        success {
            echo 'Build and publish succeeded!'
        }
        failure {
            echo 'Build failed!'
        }
    }
}
