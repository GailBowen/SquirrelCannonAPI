document.addEventListener('DOMContentLoaded', function () {
    const cards = document.querySelectorAll('.flashcard-container');
    let currentCardIndex = 0;

    //Hide all cards initially
    cards.forEach(card => {
        card.style.display = 'none';
    });

    function showCard(index) {
        // Hide all cards
        cards.forEach(card => {
            card.style.display = 'none';

            const answerInput = card.querySelector('.answer-input');

            if (answerInput) answerInput.value = '';

            const checkButton = card.querySelector('.check-answer');

            if (checkButton) checkButton.style.display = 'block';

            const answerResult = card.querySelector('.answer-result');

            if (answerResult) answerResult.style.display = 'none';

        });

        // Show current card if it exists
        if (index < cards.length) {
            cards[index].style.display = 'block';

            const input = cards[index].querySelector('.answer-input');
            if (input) {
                input.value = '';
                input.focus();
            }

            //Update progress counter
            const currentCardNumber = document.getElementById('currentCardNumber');

            if (currentCardNumber) {
                currentCardNumber.textContent = index + 1;
            }
        }
    }

    // Show first card initially
    showCard(0);

    // Handle checking answers
    document.querySelectorAll('.check-answer').forEach(button => {
        button.addEventListener('click', function () {
            const container = this.closest('.card-body');
            const cardContainer = this.closest('.flashcard-container');
            const userAnswer = container.querySelector('.answer-input').value.trim().toLowerCase();
            const correctAnswer = cardContainer.getAttribute('data-answer');
            const resultSection = container.querySelector('.answer-result');
            const correctDiv = container.querySelector('.correct-answer');
            const wrongDiv = container.querySelector('.wrong-answer');
            const correctFlag = container.querySelector('.correct-flag');

            resultSection.style.display = 'block';
            if (userAnswer === correctAnswer) {
                correctDiv.style.display = 'block';
                wrongDiv.style.display = 'none';
                correctFlag.value = 'true';
            } else {
                correctDiv.style.display = 'none';
                wrongDiv.style.display = 'block';
                correctFlag.value = 'false';
            }

            container.querySelector('.response-buttons').style.display = 'block';
            this.style.display = 'none';
        });
    });

    // Add event listeners to next card buttons
    document.querySelectorAll('.next-card').forEach(button => {
        button.addEventListener('click', function (e) {
            e.preventDefault(); // Prevent form submission

            // Get the form and submit it
            const form = this.closest('form');
            fetch(form.action, {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: new FormData(form)
            });

            currentCardIndex++;
            if (currentCardIndex < cards.length) {
                showCard(currentCardIndex);
            } else {
                // All cards completed
                document.getElementById('flashcard-deck').innerHTML =
                    '<div class="alert alert-success">All cards reviewed! Great job!</div>';
            }
        });
    });

    //Global enter key handler
    document.addEventListener('keypress', function (e) {

        if (e.key === 'Enter') {
            const currentCard = cards[currentCardIndex];
            const checkButton = currentCard.querySelector('.check-answer');
            const nextButton = currentCard.querySelector('.next-card');

            if (checkButton && checkButton.style.display !== 'none') {
                checkButton.click();
            } else if (nextButton) {
                nextButton.click();
            }

        }

    });

});
