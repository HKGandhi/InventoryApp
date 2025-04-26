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

        stage('Check Code Coverage') {
            steps {
                script {
                    def coverageFile = findFiles(glob: '**/TestResults/**/coverage.cobertura.xml')[0]
                    def coverageXml = new XmlSlurper().parse(coverageFile.path)
                    def lineRate = coverageXml.@line-rate.toDouble()
                    def lineCoveragePercent = lineRate * 100

                    echo "Line Coverage Achieved: ${lineCoveragePercent}%"

                    if (lineCoveragePercent < 50) {
                        error "❌ Code coverage too low: ${lineCoveragePercent}%. Minimum 50% required."
                    } else {
                        echo "✅ Code coverage is good: ${lineCoveragePercent}%"
                    }
                }
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
                        bat 'copy /Y out\\*.* \\\\dev-server\\deployments\\inventoryapp\\'
                    } else if (params.ENVIRONMENT == 'qa') {
                        echo "🚀 Deploying to QA server..."
                        bat 'copy /Y out\\*.* \\\\qa-server\\deployments\\inventoryapp\\'
                    } else if (params.ENVIRONMENT == 'pre-prod') {
                        echo "🚀 Deploying to PRE-PROD server..."
                        bat 'copy /Y out\\*.* \\\\preprod-server\\deployments\\inventoryapp\\'
                    } else {
                        error "❌ Invalid environment selected."
                    }
                }
            }
        }
    }

    post {
         always {

           
            publishCoverage adapters: [
                coberturaAdapter('**/TestResults/**/coverage.cobertura.xml')
            ],
                archiveArtifacts artifacts: 'out/**/*.*', fingerprint: true
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
