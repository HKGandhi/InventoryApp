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
                sh 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet build --no-restore'
            }
        }

        stage('Test') {
            steps {
                sh 'dotnet test --no-build --verbosity normal'
            }
        }

        stage('Publish') {
            steps {
                sh 'dotnet publish -c Release -o out'
            }
        }
    }

    post {
        success {
            echo 'Build and publish succeeded!'
        }
        failure {
            echo 'Build failed!'
        }
    }
}
